using System.Windows;
using System.Windows.Input;
using TestTaskEditImageWpf.Helpers;
using TestTaskEditImageWpf.Helpers.Command;
using TestTaskEditImageWpf.Model.HomePage;

namespace TestTaskEditImageWpf.ViewModel.HomePage
{
    internal class HomeViewModel : ViewModel<HomeModel>
    {
        private HomeModel _model;

        private ICommand _addPersonCommand;
        private ICommand _deletePersonCommand;
        private ICommand _starWorkingOnImageCommand;
        public HomeViewModel()
        {
            _model = new HomeModel();
            _people = new List<Person>();
            _workGroup = new WorkGroup();
            _selectedPerson = new Person();
            _peopleComplateTasks = new List<Person>();

            _name = string.Empty;
            _totalImage = 0;
            _totalTimeForComplateImages = 0;
            _totalCompateImage = 0;

            _addPersonCommand = new DelegateCommand(AddPerson);
            _deletePersonCommand = new DelegateCommand(DeletePerson);
            _starWorkingOnImageCommand = new DelegateCommand(StartWorkingOnImage);
        } 

        private WorkGroup _workGroup;
        public WorkGroup WorkGroup
        {
            get { return _workGroup; }
            set { _workGroup = value; OnPropertyChanged(nameof(WorkGroup)); }
        }

        private int _totalCompateImage;
        public int TotalCompateImage
        {
            get { return _totalCompateImage; }
            set { _totalCompateImage = value; OnPropertyChanged(nameof(TotalCompateImage)); }
        }

        private int _totalTimeForComplateImages;
        public int TotalTimeForComplateImages
        {
            get { return _totalTimeForComplateImages; }
            set { _totalTimeForComplateImages = value; OnPropertyChanged(nameof(TotalTimeForComplateImages)); }
        }

        private int _totalImage;
        public int TotalImage
        {
            get { return _totalImage; }
            set { _totalImage = value; OnPropertyChanged(nameof(TotalImage));}
        } 
        private int _id;
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(nameof(Name));}
        }
        private int _timeWorkForOneImage;
        public int TimeWorkForOneImage
        {
            get { return _timeWorkForOneImage; }
            set { _timeWorkForOneImage = value; OnPropertyChanged(nameof(TimeWorkForOneImage)); }
        }
        private Person _selectedPerson;
        public Person SelectedPerson
        {
            get { return _selectedPerson; }
            set { _selectedPerson = value; SelectedItem(value); }
        }
        private List<Person> _people;
        public List<Person> People
        {
            get { return _people; }
            set { _people = value; OnPropertyChanged(nameof(People)); }
        }

        private List<Person> _peopleComplateTasks;
        public List<Person> PeopleComplateTasks
        {
            get { return _peopleComplateTasks; }
            set { _peopleComplateTasks = value; OnPropertyChanged(nameof(PeopleComplateTasks));}
        }

        public ICommand AddPersonCommand => _addPersonCommand;
        private void AddPerson()
        {
            try
            {
                List<Person> list = new List<Person>();
                foreach (Person item in _people)
                {
                    list.Add(item);
                }
                Person person = new Person(Name, _timeWorkForOneImage);
                list.Add(person);
                People = list;

                Name = string.Empty;
                TimeWorkForOneImage = 0;
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            } 
        }

        public ICommand DeletePersonCommand => _deletePersonCommand;
        private void DeletePerson() 
        {
            try
            {
                List<Person> list = new List<Person>();
                foreach (Person item in _people)
                {
                    list.Add(item);
                }
                var removeItem = list.Where(item => item.Id == _id).First();
                list.Remove(removeItem);
                People = list;

                Name = string.Empty;
                TimeWorkForOneImage = 0;
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SelectedItem(Person person)
        {
            try
            {
                if (person != null)
                {
                    Name = person.Name;
                    TimeWorkForOneImage = person.TimeWorkForOneImage;
                    _id = person.Id;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
        }

        public ICommand StartWorkingOnImageCommand => _starWorkingOnImageCommand;
        private void StartWorkingOnImage()
        {
            try
            {
                if (People.Count == 0)
                {
                    throw new Exception("Список людей які готові виконувати завдання 0");
                }
                if (TotalImage == 0)
                {
                    throw new Exception("Список завдань 0");
                }
                Task t = Task.Run(async () =>
                {

                    foreach (Person person in People)
                    {
                        person.CompleteImage = 0;
                        person.TotalTimeWorkForImages = 0;
                    }

                    _workGroup = new WorkGroup(People, TotalImage);
                    await _workGroup.Work();
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        PeopleComplateTasks = People;
                        TotalTimeForComplateImages = _workGroup.TotalTimeWorkForImage;
                        TotalCompateImage = _workGroup.DoneImages.Count;

                        TotalImage = 0;
                    });
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
