using Parsers;
using Saver;
using System.Text;
using LogLibrary;
using System.Linq;
using System.IO;
using CommunityToolkit.Maui.Storage;
 
using System.Threading; 


namespace MauiApp26
{
	public partial class MainPage : ContentPage
	{
		string _filePath;
        RatingService _ratingService = new RatingService(new DOMParsingStrategy());
        List<Student> results = new List<Student>(); 
        List<Student> resultsEdited = new List<Student>();
        List<Student> resultsHTML = new List<Student>();
        Logger logger = Logger.Instance;
		public MainPage()
		{
			InitializeComponent();
		}
		private async void OnLoadFileClicked(object sender, EventArgs e)
		{
			string? filePath = await FileLoader.SelectFileAsync();
			if (!string.IsNullOrEmpty(filePath))
			{
				_filePath = filePath;
				selectedLbl.Text = $"Обраний файл: {_filePath}";
				try
				{

					results = (List<Student>)(object)_ratingService.LoadRating(_filePath);
					ParsedTableGrid.Children.Clear();
					var RatingGrid = CreateRatingGrid(results);
					ParsedTableGrid.Children.Add(RatingGrid);

					FacultyPicker.IsEnabled = true;
					DepartmentPicker.IsEnabled = true;
					CoursePicker.IsEnabled = true;
					NamePicker.IsEnabled = true;
					SubjectPicker.IsEnabled = true;

					FacultyPicker.ItemsSource = new List<string>();
					DepartmentPicker.ItemsSource = new List<string>();
					CoursePicker.ItemsSource = new List<string>();
					NamePicker.ItemsSource = new List<string>();
					SubjectPicker.ItemsSource = new List<string>();

					resultsEdited = results.ToList();
					resultsHTML = results.ToList();

					List<string> faculties = new List<string>();
					List<string> departments = new List<string>();
					List<string> courses = new List<string>();
					List<string> fullNames = new List<string>();
					List<string> disciplines = new List<string>();

					foreach (var res in resultsEdited)
					{
						faculties.Add(res.Faculty);
						departments.Add(res.Department);
						courses.Add(res.Course);
						fullNames.Add(res.FullName);
						foreach (var grade in res.Grades)
						{
							disciplines.Add(grade.Subject);
						}
					}

					faculties.Add("all");
					departments.Add("all");
					courses.Add("all");
					fullNames.Add("all");
					disciplines.Add("all");

					FacultyPicker.ItemsSource = new HashSet<string>(faculties).Where(x => !string.IsNullOrEmpty(x)).ToList();
					DepartmentPicker.ItemsSource = new HashSet<string>(departments).Where(x => !string.IsNullOrEmpty(x)).ToList();
					CoursePicker.ItemsSource = new HashSet<string>(courses).Where(x => !string.IsNullOrEmpty(x)).ToList();
					NamePicker.ItemsSource = new HashSet<string>(fullNames).Where(x => !string.IsNullOrEmpty(x)).ToList();
					SubjectPicker.ItemsSource = new HashSet<string>(disciplines).Where(x => !string.IsNullOrEmpty(x)).ToList();

				}
				catch (Exception ex)
				{
					await DisplayAlert("Помилка завантаження", ex.Message, "OK");
				}

			}
			else
			{
				FacultyPicker.IsEnabled = false;
				DepartmentPicker.IsEnabled = false;
				CoursePicker.IsEnabled = false;
				NamePicker.IsEnabled = false;
				SubjectPicker.IsEnabled = false;
			}
		}
		public static Grid CreateRatingGrid(List<Student> students)
		{
			var grid = new Grid
			{
				ColumnDefinitions =
				{
					new ColumnDefinition { Width = GridLength.Auto },
					new ColumnDefinition { Width = GridLength.Auto },
					new ColumnDefinition { Width = GridLength.Auto },
					new ColumnDefinition { Width = GridLength.Auto },
					new ColumnDefinition { Width = GridLength.Auto }
				}
			};
			AddGridHeader(grid, "П.І.П.", 0);
			AddGridHeader(grid, "Факультет", 1);
			AddGridHeader(grid, "Кафедра", 2);
			AddGridHeader(grid, "Курс", 3);
			AddGridHeader(grid, "Успішність (Дисц.: Оцінка)", 4);

			int row = 1;
			foreach (var student in students)
			{
				var gradesText = student.AllGradesDisplay; 

				grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

				AddGridCell(grid, student.FullName, row, 0);
				AddGridCell(grid, student.Faculty, row, 1);
				AddGridCell(grid, student.Department, row, 2);
				AddGridCell(grid, student.Course, row, 3);
				AddGridCell(grid, gradesText, row, 4);

				row++;
			}
			grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

			for (int col = 0; col < grid.ColumnDefinitions.Count; col++)
			{
				AddGridCell(grid, string.Empty, row, col);
			}

			return grid;
		}
		private static void AddGridHeader(Grid grid, string header, int column)
		{
			var label = new Label
			{
				Text = header,
				FontAttributes = FontAttributes.Bold,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center,
				Padding = new Thickness(5, 5)
			};

			grid.Children.Add(label);
			Grid.SetRow(label, 0);
			Grid.SetColumn(label, column);
		}
		private static void AddGridCell(Grid grid, string text, int row, int column)
		{
			var label = new Label
			{
				Text = text,
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.Center,
				Padding = new Thickness(5, 5)
			};

			grid.Children.Add(label);
			Grid.SetRow(label, row);
			Grid.SetColumn(label, column);
		}
		private async void OnHelpClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Довідка", "Виконала Погоріла Анна К-25", "OK");
        }

		private async void OnExitClicked(object sender, EventArgs e)
		{
			if (await DisplayAlert("Вихід", "Чи дійсно ви хочете завершити роботу з програмою?", "Так", "Ні"))
			{
				Application.Current?.Quit();
			}
		}
		private async void OnParserTypeChanged(object sender, CheckedChangedEventArgs e)
		{
			if (string.IsNullOrEmpty(_filePath))
			{
				await DisplayAlert("Помилка", $"Оберіть файл перед пошуком.", "OK");
				return;
			}
			if (rbDOM.IsChecked == true)
				_ratingService = new RatingService(new DOMParsingStrategy());
			else if (rbLINQ.IsChecked == true)
				_ratingService = new RatingService(new LINQParsingStrategy());
			else if (rbSAX.IsChecked == true)
				_ratingService = new RatingService(new SAXParsingStrategy());

			try
			{
				results = (List<Student>)(object)_ratingService.LoadRating(_filePath);
			}
			catch (Exception ex)
			{
				await DisplayAlert("Помилка парсера", ex.Message, "OK");
				return;
			}


			FacultyPicker.SelectedItem = null;
			DepartmentPicker.SelectedItem = null;
			SubjectPicker.SelectedItem = null;
			NamePicker.SelectedItem = null;
			CoursePicker.SelectedItem = null;

			ParsedTableGrid.Children.Clear();
			var ratingGrid = CreateRatingGrid(results);
			ParsedTableGrid.Children.Add(ratingGrid);
		}
		private void OnFacultySelected(object sender, EventArgs e)
		{
			OnSelected();
		}
		private void OnDepartmentSelected(object sender, EventArgs e)
        {
            OnSelected();
        }
        
        private void OnSubjectSelected(object sender, EventArgs e)
        {
            OnSelected();
        }
        
        private void OnCourseSelected(object sender, EventArgs e)
        {
            OnSelected();
        }
        
        private void OnNameSelected(object sender, EventArgs e)
        {
            OnSelected();
        }

		private void OnSelected()
		{

			var selectedFaculty = FacultyPicker.SelectedItem?.ToString();
			var selectedDepartment = DepartmentPicker.SelectedItem?.ToString();
			var selectedCourse = CoursePicker.SelectedItem?.ToString();
			var selectedFullName = NamePicker.SelectedItem?.ToString();
			var selectedSubject = SubjectPicker.SelectedItem?.ToString();

			string select = "Параметри: ";

			resultsEdited = results.ToList();

			if (!string.IsNullOrEmpty(selectedFaculty) && selectedFaculty != "all")
			{
				resultsEdited.RemoveAll(res => res.Faculty != selectedFaculty);
				select += ($"Факультет = \"{selectedFaculty}\",");
			}
			if (!string.IsNullOrEmpty(selectedDepartment) && selectedDepartment != "all")
			{
				resultsEdited.RemoveAll(res => res.Department != selectedDepartment);
				select += ($"Кафедра = \"{selectedDepartment}\",");
			}
			if (!string.IsNullOrEmpty(selectedCourse) && selectedCourse != "all")
			{
				resultsEdited.RemoveAll(res => res.Course != selectedCourse);
				select += ($"Курс = \"{selectedCourse}\",");
			}
			if (!string.IsNullOrEmpty(selectedFullName) && selectedFullName != "all")
			{
				resultsEdited.RemoveAll(res => res.FullName != selectedFullName);
				select += ($"П.І.П. = \"{selectedFullName}\",");
			}
			if (!string.IsNullOrEmpty(selectedSubject) && selectedSubject != "all")
			{
				resultsEdited.RemoveAll(student => !student.Grades.Any(grade => grade.Subject == selectedSubject));
				select += ($"Дисципліна = \"{selectedSubject}\",");
			}

			ParsedTableGrid.Children.Clear();
			var ratingGrid = CreateRatingGrid(resultsEdited);
			ParsedTableGrid.Children.Add(ratingGrid);

			resultsHTML = resultsEdited.ToList();

			logger.Log("Фільтрація", select);
		}
		private void OnClearClicked(object sender, EventArgs e)
		{
			FacultyPicker.SelectedItem = null;
			DepartmentPicker.SelectedItem = null;
			SubjectPicker.SelectedItem = null;
			NamePicker.SelectedItem = null;
			CoursePicker.SelectedItem = null;
			OnSelected();
		}
		private async void OnHTMLButtonClicked(object sender, EventArgs e)
{
    if (string.IsNullOrEmpty(_filePath))
    {
        await DisplayAlert("Помилка", "Файл не обрано", "OK");
        return;
    }

    string selectedFormat = await DisplayActionSheet("Оберіть формат збереження", "Скасувати", null, "HTML", "XML");

    if (selectedFormat == "Скасувати" || string.IsNullOrEmpty(selectedFormat))
        return;
    
    try
    {
        var saver = Saver.SaverFactory.GetSaver(selectedFormat);
        string fileContent = saver.GenerateContent(resultsHTML.Cast<Parsers.Student>().ToList()); 

        string fileName = selectedFormat == "XML" ? "students_filtered.xml" : "students_filtered.html";
        
        using var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(fileContent));
        

        var result = await global::CommunityToolkit.Maui.Storage.FileSaver.SaveAsync(
            fileName, 
            stream, 
            new System.Threading.CancellationToken()
        );
            
        if (!string.IsNullOrEmpty(result.FilePath))
        {
            logger.Log("Збереження", $"Файл збережено у форматі {selectedFormat} за шляхом: {result.FilePath}");
            await DisplayAlert("Успіх", $"Файл успішно збережено.\nШлях: {result.FilePath}", "OK");
        }
        else
        {

            logger.Log("Збереження", "Збереження скасовано користувачем.");
        }
    }
    catch (Exception ex)
    {
        logger.Log("Помилка збереження", ex.Message);
        await DisplayAlert("Помилка", $"Не вдалося зберегти файл: {ex.Message}", "OK");
    }
}
	}
}


