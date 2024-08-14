using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhomB4.Controllers.Builder
{
    public class Builder_Pattern
    {
        //Xây dựng chi tiết cây cảnh
        public class Plant
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string ImageUrl { get; set; }
        }

        // Builder interface
        public interface IPlantBuilder
        {
            void SetName(string name);
            void SetDescription(string description);
            void SetImageUrl(string imageUrl);
            Plant Build();
        }

        // Concrete builder
        public class PlantBuilder : IPlantBuilder
        {
            private Plant _plant = new Plant();

            public void SetName(string name)
            {
                _plant.Name = name;
            }

            public void SetDescription(string description)
            {
                _plant.Description = description;
            }

            public void SetImageUrl(string imageUrl)
            {
                _plant.ImageUrl = imageUrl;
            }

            public Plant Build()
            {
                return _plant;
            }
        }

        // Director
        public class PlantDirector
        {
            private readonly IPlantBuilder _plantBuilder;

            public PlantDirector(IPlantBuilder plantBuilder)
            {
                _plantBuilder = plantBuilder;
            }

            public void Construct(string name, string description, string imageUrl)
            {
                _plantBuilder.SetName(name);
                _plantBuilder.SetDescription(description);
                _plantBuilder.SetImageUrl(imageUrl);
            }
        }

        public class PlantController : Controller
        {
            public ActionResult Create(string name, string description, string imageUrl)
            {
                IPlantBuilder plantBuilder = new PlantBuilder();
                var plantDirector = new PlantDirector(plantBuilder);

                plantDirector.Construct(name, description, imageUrl);

                var plant = plantBuilder.Build();

                return RedirectToAction("Index", "Home");
            }
        }

    }
}