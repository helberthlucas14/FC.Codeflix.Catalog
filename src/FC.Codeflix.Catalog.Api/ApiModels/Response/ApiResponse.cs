﻿using FC.Codeflix.Catalog.Application.Common;

namespace FC.Codeflix.Catalog.Api.ApiModels.Response
{
    public class ApiResponse<TData>
    {
        public TData Data { get; private set; }

        public ApiResponse(TData data)
            => Data = data;
    }

    public class ApiResponseList<TItemData> : ApiResponse<IReadOnlyList<TItemData>>
    {
        public ApiResponseListMeta Meta { get; private set; }

        public ApiResponseList(PaginatedListOutput<TItemData> paginatedListOutput)
            : base(paginatedListOutput.Items)
        {
            Meta = new(
                paginatedListOutput.Page,
                paginatedListOutput.PerPage,
                paginatedListOutput.Total
                );
        }
    }
    public class ApiResponseListMeta
    {
        public int CurrentPage { get; set; }
        public int PerPage { get; set; }
        public int Total { get; set; }
        public ApiResponseListMeta(int currentPage, int perPage, int total)
        {
            CurrentPage = currentPage;
            PerPage = perPage;
            Total = total;
        }


    }
}
