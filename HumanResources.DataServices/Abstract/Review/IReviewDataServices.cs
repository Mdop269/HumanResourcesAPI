using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourcesAPI.EntityData.EntityModels;

namespace HumanResources.DataServices.Abstract
{
    public interface IReviewDataServices
    {
        Task<List<Review>> GetAllReview();

        Task<Review> GetReviewById(int reviewID);

        Task<Review> AddReview(Review review);

        Task<Review> UpdateReview(Review review);

        Task<bool> DeleteReview(Review review);

        Task<Review> GetReviewByUnique(Review review);

    }
}
