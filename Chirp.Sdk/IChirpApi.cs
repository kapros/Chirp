using Chirp.Contracts.V1;
using Chirp.Contracts.V1.Requests;
using Chirp.Contracts.V1.Responses;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Chirp.Sdk
{
    [Headers("Authorization: Bearer")]
    public interface IChirpApi
    {
        [Get(ApiRoutes.Posts.Get)]
        Task<ApiResponse<List<PostResponse>>> GetAllPostsAsync();

        [Get(ApiRoutes.Posts.GetById)]
        Task<ApiResponse<PostResponse>> GetPostAsync(Guid id);

        [Post(ApiRoutes.Posts.Create)]
        Task<ApiResponse<PostResponse>> CreatePostAsync([Body] CreatePostRequest request);

        [Put(ApiRoutes.Posts.Update)]
        Task<ApiResponse<PostResponse>> UpdatePostAsync(Guid id, [Body] UpdatePostRequest request);

        [Delete(ApiRoutes.Posts.Delete)]
        Task<ApiResponse<PostResponse>> DeletePostAsync(Guid id);
    }
}
