using MediatR;
using Microsoft.AspNetCore.Components;
using OnlineShop.Application.Common;
using OnlineShop.Domain.DTOs;
using OnlineShop.Infrastructure.Products.Queries;

public class ProductsBase : ComponentBase
{
    [Inject] IMediator Mediator { get; set; }
    
    protected PagedList<ProductDTO> Products;
    protected string _searchString;
    protected int _page = 1;
    protected int _pageSize = 10;

    protected override async Task OnInitializedAsync()
    {
        await LoadProducts();
    }

    protected async Task LoadProducts()
    {
        var query = new GetProductsQuery(_searchString, _page, _pageSize);
        Products = await Mediator.Send(query);
    }

    protected async void PageChanged(int page)
    {
        _page = page;
        await LoadProducts();
    }
}