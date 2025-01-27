using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResources.DataServices.Abstract;
using HumanResources.Manager.DTOs;
using HumanResources.Manager.Validation.Employee;
using HumanResources.Manager.Validation.Review;

namespace HumanResources.Manager.Managers.Review
{
    public class ReviewManager
    {
        //this is better approach for getting thw date rather then new date() in this if the system date doesnt update it will be a prob
        private readonly DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);

        private readonly IEmployeeDataServices _employeeDataServices;
        private readonly ITeamsDataServices _teamsDataServices;
        private readonly IReviewDataServices _reviewDataServices;


        public ReviewManager(
            IEmployeeDataServices employeeDataServices,
            ITeamsDataServices teamsDataServices,
            IReviewDataServices reviewDataServices


            )
        {
            _employeeDataServices = employeeDataServices;
            _teamsDataServices = teamsDataServices;
            _reviewDataServices = reviewDataServices;
        }

        public async Task<List<ReviewDTO>> GetAllReview()
        {
            var result = await _reviewDataServices.GetAllReview();

            return result.Select(x => ReviewDTO.MapToDTO(x)).ToList();
        }

        public async Task<ReviewDTO> GetReviewById(int reviewID)
        {
            var validation = new Dictionary<string, string>();

            if (reviewID <= 0)
                validation.Add("reviewID", "Invalid reviewID");

            if (validation.Count == 0)
            {

                var result = await _reviewDataServices.GetReviewById(reviewID);
                if (result is null)
                {
                    throw new ReviewValidationException($"Review with ID {reviewID} does not exist.");
                }
                return ReviewDTO.MapToDTO(result);

            }
            else
            {
                throw new ReviewValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
            }
        }

        public async Task<String> UpsertReview(ReviewDTO reviewDTO)
        {
            var validation = new Dictionary<string, string>();
            if (reviewDTO is null)
            {
                validation.Add("Null", "Employee Cant Be Null");
            }
            else if (string.IsNullOrWhiteSpace(reviewDTO.Comments) || reviewDTO.Comments.Length < 15 )
            {
                validation.Add("EmptyComment", "Comment Cant Be Null or WhiteSpace or length cant be below 15 ");

            }
            else if (reviewDTO.PerformanceRating > 10 || reviewDTO.PerformanceRating < 0)
            {
                validation.Add("Rating is invalid", "Fronm 0 to 10 only you can rate");
            }
            else if (reviewDTO.EmployeeId <= 0 || await _employeeDataServices.GetEmployeeById(reviewDTO.EmployeeId) == null)
            {
                validation.Add("WrongEmployeeId", "EmployeeId is Invalid");
            }
            else if (reviewDTO.ReviewedBy <= 0 || await _employeeDataServices.GetEmployeeById(reviewDTO.ReviewedBy) == null)
            {
                validation.Add("WrongReviewedBy", "ReviewedBy is Invalid");
            }
            // we use int becuse its optional thats why
            else if (reviewDTO.TeamId <= 0 && await _teamsDataServices.GetTeamById((int)reviewDTO.TeamId) == null)
            {
                validation.Add("WrongTeamId", "TeamId is Invalid");
            }

            if (validation.Count == 0)
            {
                var reviewEntity = ReviewDTO.MapToEntity(reviewDTO);

                var checkingData = await _reviewDataServices.GetReviewByUnique(reviewEntity);
                if (checkingData is not null)
                {


                    checkingData.TeamId = reviewDTO.TeamId;
                    checkingData.Comments = reviewDTO.Comments;
                    checkingData.PerformanceRating = reviewDTO.PerformanceRating;

                    var updatedData = await _reviewDataServices.UpdateReview(reviewEntity);
                    return "Updated";
                       
                    
                }
                else
                {
                    
                    reviewEntity.ReviewDate = currentDate;
                    var addData = await _reviewDataServices.AddReview(reviewEntity);

                    return "Added";
                }
            }
            else
            {
                throw new ReviewValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));

            }
        }

        public async Task<bool> DeleteReview(int reviewID)
        {
            var validation = new Dictionary<string, string>();

            if (reviewID <= 0)
            {
                validation.Add("Enter Valid Details", " reviewID  Is Invalid");
            }
            else if (await _reviewDataServices.GetReviewById(reviewID) == null)
            {
                validation.Add("Employee Invalid", $" Employee with {reviewID} is Invalid");
            }
            if (validation.Count == 0)
            {
                var reviewData = await _reviewDataServices.GetReviewById(reviewID);


                var result = await _reviewDataServices.DeleteReview(reviewData);

                return true;
            }
            else
            {
                throw new ReviewValidationException("Validation failed: " + string.Join(", ", validation.Select(kv => $"{kv.Key}: {kv.Value}")));
            }
        }
    }
}
