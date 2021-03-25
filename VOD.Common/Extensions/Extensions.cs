using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VOD.Common.Extensions
{
    public static class Extensions
    {
        public static SelectList ToSelectList<TDto>(this List<TDto> items, string valuefield, string textfield)
            where TDto : class
        {
            return new SelectList(items, valuefield, textfield);
        }
    }
}