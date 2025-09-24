using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskEditImageWpf.Helpers
{
    public class Person
    {
        public int Id {  get; set; }
        public string Name { get; set; }

        public int TimeWorkForOneImage { get; set; }

        public int CompleteImage { get; set; }

        public int TotalTimeWorkForImages { get; set; }

        public Person() 
        {
            Name = string.Empty;
            CompleteImage = 0;
            TotalTimeWorkForImages = 0;
            Id = 0;
        }

        public Person(string name, int timeWorkForOneImage)
        {
            Name = name;
            TimeWorkForOneImage = timeWorkForOneImage;
            CompleteImage = 0;
            TotalTimeWorkForImages = 0;
            Id = new Random().Next();
        }

        internal async Task<Image> WorkInOneImage(Image image)
        {
            await Task.Delay(TimeWorkForOneImage);

            image.IsWorkDone = true;
            CompleteImage += 1;
            TotalTimeWorkForImages += TimeWorkForOneImage;

            return image;
        }
    }
}
