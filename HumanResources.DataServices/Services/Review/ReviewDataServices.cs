using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResources.DataServices.Abstract;
using HumanResourcesAPI.EntityData.EntityModels;
using HumanResourcesAPI.EntityData;
using Microsoft.EntityFrameworkCore;

namespace HumanResources.DataServices.Services
{
    public class ReviewDataServices : IReviewDataServices
    {
        private readonly HumanResourcesContext _context;

        public ReviewDataServices(HumanResourcesContext context)
        {
            _context = context;
        }

        public async Task<List<Review>> GetAllReview()
        {
            var result = await _context.Reviews.ToListAsync();

            return result;
        }

        public async Task<Review> GetReviewById(int reviewID)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(a => a.ReviewId == reviewID);

            return review;
        }

        public async Task<Review> AddReview(Review review)
        {
            await _context.Reviews.AddAsync(review);

            await _context.SaveChangesAsync();


            return review;
        }

        public async Task<Review> UpdateReview(Review review)
        {
            await _context.SaveChangesAsync();

            return review;
        }


        public async Task<bool> DeleteReview(Review review)
        {
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Review> GetReviewByUnique(Review review)
        {
            var existingReview = await _context.Reviews.FirstOrDefaultAsync(a => a.EmployeeId == review.EmployeeId && a.ReviewedBy == review.ReviewedBy && a.ReviewDate == a.ReviewDate);

            return existingReview;
        }

        
    }
}
