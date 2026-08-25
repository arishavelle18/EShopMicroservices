using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Discount.Grpc.Protos;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService(DiscountContext discountContext, ILogger<DiscountService> logger) : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var cancellationToken = context.CancellationToken;
        var getDiscountByProductName = await discountContext.Coupons.FirstOrDefaultAsync(a => a.ProductName.ToLower() == request.ProductName.ToLower(), cancellationToken);

        if(getDiscountByProductName is null)
        {
            getDiscountByProductName = new Coupon { Id=Guid.Empty.ToString(), ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };
        }
        logger.LogInformation("Discount is retrieved for ProductName : {ProductName}, Amount : {Amount}", getDiscountByProductName.ProductName, getDiscountByProductName.Amount);
        var couponModel = getDiscountByProductName.Adapt<CouponModel>();
        return couponModel;
    }
    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        // check if product name is existing
        var cancellationToken = context.CancellationToken;
        var coupon = request.Coupon.Adapt<Coupon>();
        if(coupon is null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, $"Invalid coupon data provided."));
        }

        var convertToDto = Coupon.Create(coupon.ProductName, coupon.Description, coupon.Amount);
        await discountContext.AddAsync(convertToDto, cancellationToken: cancellationToken);
        await discountContext.SaveChangesAsync(cancellationToken: cancellationToken);
        logger.LogInformation("Discount is successfully created. Product Name: {ProductName}", coupon.ProductName);
        return convertToDto.Adapt<CouponModel>();
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var cancellationToken = context.CancellationToken;
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon is null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, $"Invalid coupon data provided."));
        }
        //check if the coupon exists
        var getDiscountById = await discountContext.Coupons.FirstOrDefaultAsync(a => a.Id.ToLower() == coupon.Id.ToLower(), cancellationToken);

        if (getDiscountById is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={coupon.ProductName} not found."));
        }
        else
        {
            //update
            getDiscountById.Update(coupon.ProductName, coupon.Description, coupon.Amount);

            discountContext.Update(getDiscountById);
            await discountContext.SaveChangesAsync(cancellationToken: cancellationToken);
            logger.LogInformation("Discount is successfully updated. Product Name: {ProductName}", coupon.ProductName);
            return new CouponModel { Id = getDiscountById!.Id.ToString(), ProductName = getDiscountById.ProductName, Description = getDiscountById.Description, Amount = getDiscountById.Amount };
        }

       

    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var cancellationToken = context.CancellationToken;
        //check if the coupon exists
        var getDiscountById = await discountContext.Coupons.FirstOrDefaultAsync(a => a.Id.ToLower() == request.Id.ToLower(), cancellationToken);

        if (getDiscountById is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.Id} not found."));
        }

        //delete
        discountContext.Coupons.Remove(getDiscountById);
        await discountContext.SaveChangesAsync(cancellationToken: cancellationToken);

        return new DeleteDiscountResponse { IsSuccess = true  };
    }
}
