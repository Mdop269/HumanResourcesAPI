using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourcesAPI.EntityData.EntityModels;

namespace HumanResources.Manager.DTOs
{
    public class ReviewDTO
    {
        public int TeamId { get; set; }

        public int EmployeeId { get; set; }

        public int ReviewedBy { get; set; }

        public DateOnly ReviewDate { get; set; }

        public string Comments { get; set; } = null!;

        public decimal PerformanceRating { get; set; }

        public static Review MapToEntity(ReviewDTO reviewDTO)
        {
            return new Review
            {
                TeamId = reviewDTO.TeamId,
                EmployeeId = reviewDTO.EmployeeId,
                ReviewedBy = reviewDTO.ReviewedBy,
                ReviewDate = reviewDTO.ReviewDate,
                Comments = reviewDTO.Comments,
                PerformanceRating = reviewDTO.PerformanceRating,
            };
        }

        public static ReviewDTO MapToDTO(Review review) => new ReviewDTO
        {

            TeamId = review.TeamId,
            EmployeeId = review.EmployeeId,
            ReviewedBy = review.ReviewedBy,
            ReviewDate = review.ReviewDate,
            Comments = review.Comments,
            PerformanceRating = review.PerformanceRating,
        };
    }
}
