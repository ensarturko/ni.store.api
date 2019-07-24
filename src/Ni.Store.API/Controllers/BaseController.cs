using Ni.Store.Api.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Ni.Store.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        private const string OffsetPattern = @"Offset=";

        public void PreparePagination<T>(int offset, int limit, long total, string resource, PagedApiResponse<List<T>> response)
        {
            response.Total = total;
            response.PageSize = limit;

            string queryString = string.Empty;
            string baseQuery = $"api/{resource}";

            if (HttpContext.Request.QueryString.HasValue)
            {
                queryString = HttpContext.Request.QueryString.Value;
            }

            int tmpNextOffset = (offset + limit) + 1;
            if (tmpNextOffset <= total)
            {
                string nextPage = queryString.Replace($"{OffsetPattern}{offset}", $"{OffsetPattern}{tmpNextOffset}");
                response.Next = $"{baseQuery}{nextPage}";
            }

            int tmpPrevOffset = (offset - limit) - 1;
            if (tmpPrevOffset >= 0)
            {
                string prevPage = queryString.Replace($"{OffsetPattern}{offset}", $"{OffsetPattern}{tmpPrevOffset}");
                response.Prev = $"{baseQuery}{prevPage}";
            }

            int tmpTotalPageOffset = (int)(total - limit) + 1;
            if (total > 0)
            {
                string lastPage = queryString.Replace($"{OffsetPattern}{offset}", $"{OffsetPattern}{tmpTotalPageOffset}");
                response.Last = $"{baseQuery}{lastPage}";
            }

            string tmpFirstPage = queryString.Replace($"{OffsetPattern}{offset}", $"{OffsetPattern}0");
            response.First = $"{baseQuery}{tmpFirstPage}";
        }
    }
}