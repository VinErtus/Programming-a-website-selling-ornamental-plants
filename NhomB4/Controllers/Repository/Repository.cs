using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhomB4.Controllers.Repository
{
    public class Repository
    {
        //Xóa sản phẩm
        public interface IPlantRepository
        {
            void DeletePlant(int id);
        }

        public class YourDbContext : DbContext
        {
            public DbSet<Plant> Plants { get; set; }
        }

        public class PlantRepository : IPlantRepository
        {
            private readonly YourDbContext _dbContext;

            public PlantRepository(YourDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public void DeletePlant(int id)
            {
                var plantToDelete = _dbContext.Plants.Find(id);
                if (plantToDelete != null)
                {
                    _dbContext.Plants.Remove(plantToDelete);
                    _dbContext.SaveChanges();
                }
            }
        }

        public class PlantController : Controller
        {
            private readonly IPlantRepository _plantRepository;

            public PlantController(IPlantRepository plantRepository)
            {
                _plantRepository = plantRepository;
            }

            public ActionResult Delete(int id)
            {
                _plantRepository.DeletePlant(id);
                return RedirectToAction("Index", "Home");
            }
        }
    }
}