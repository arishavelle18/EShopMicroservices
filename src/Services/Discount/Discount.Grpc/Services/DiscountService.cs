using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Discount.Grpc.Protos;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService(DiscountContext discountContext, ILogger<DiscountService> logger) : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var cancellationToken = context.CancellationToken;
        var getDiscountByProductName = await discountContext.Coupons.FirstOrDefaultAsync(a => a.ProductName.ToLower() == request.ProductName.ToLower() && a.Id.ToLower() == request.Id.ToLower(), cancellationToken);

        if(getDiscountByProductName is null)
        {
            return new CouponModel { Id = "N/A", ProductName = "No Discount", Description = "No Discount Description", Amount = 0 };
        }
        logger.LogInformation("Discount is retrieved for ProductName : {ProductName}, Amount : {Amount}", getDiscountByProductName.ProductName, getDiscountByProductName.Amount);
        return new CouponModel { Id = getDiscountByProductName!.Id.ToString(),ProductName = getDiscountByProductName.ProductName,Description = getDiscountByProductName.Description, Amount = getDiscountByProductName.Amount };
    }
    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        // check if product name is existing
        var cancellationToken = context.CancellationToken;
        var getDiscountByProductName = await discountContext.Coupons.AnyAsync(a => a.ProductName.ToLower() == request.ProductName.ToLower(), cancellationToken);

        if(getDiscountByProductName)
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, $"Discount with ProductName={request.ProductName} already exists."));
        }
        var convertToDto = Coupon.Create(request.ProductName, request.Description, request.Amount);
        await discountContext.AddAsync(convertToDto, cancellationToken: cancellationToken);
        await discountContext.SaveChangesAsync(cancellationToken: cancellationToken);

        return new CouponModel { Id = convertToDto!.Id.ToString(), ProductName = convertToDto.ProductName, Description = convertToDto.Description, Amount = convertToDto.Amount };
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var cancellationToken = context.CancellationToken;
        //check if the coupon exists
        var getDiscountById = await discountContext.Coupons.FirstOrDefaultAsync(a => a.Id.ToLower() == request.Id.ToLower(), cancellationToken);

        if (getDiscountById is null)
        {
            return new CouponModel { Id = "N/A", ProductName = "No Discount", Description = "No Discount Description", Amount = 0 };
        }

        //update
       getDiscountById.Update(request.ProductName, request.Description, request.Amount);

        discountContext.Update(getDiscountById);
        await discountContext.SaveChangesAsync(cancellationToken: cancellationToken);

        return new CouponModel { Id = getDiscountById!.Id.ToString(), ProductName = getDiscountById.ProductName, Description = getDiscountById.Description, Amount = getDiscountById.Amount };

    }

    public override async Task<CouponModel> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var cancellationToken = context.CancellationToken;
        //check if the coupon exists
        var getDiscountById = await discountContext.Coupons.FirstOrDefaultAsync(a => a.Id.ToLower() == request.Id.ToLower(), cancellationToken);

        if (getDiscountById is null)
        {
            return new CouponModel { Id = "N/A", ProductName = "No Discount", Description = "No Discount Description", Amount = 0 };
        }

        //delete
        discountContext.Coupons.Remove(getDiscountById);
        await discountContext.SaveChangesAsync(cancellationToken: cancellationToken);

        return new CouponModel
        {
            Id = getDiscountById.Id.ToString(),
            ProductName = getDiscountById.ProductName,
            Description = getDiscountById.Description,
            Amount = getDiscountById.Amount
        };
    }
}
