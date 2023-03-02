using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using MALM.Model;

namespace MALM.ViewModel
{
	public class MainPageViewModel : INotifyPropertyChanged
	{
		private int _count = 0;

		private ObservableCollection<MALMItem> _items;



		public event PropertyChangedEventHandler PropertyChanged;

		public ICommand AddMALMCommand { get; }

		public ICommand ToggleCompletionCommand { get; }


		public MainPageViewModel()
		{
			MALMItems = [];
			AddMALMCommand = new Command(AddMALM);
			ToggleCompletionCommand = new Command<MALMItem>(ToggleCompletion);
		}


		public ObservableCollection<MALMItem> MALMItems
		{
			get => _items;
			set
			{
				_items = value;
				OnPropertyChanged();
			}
		}


		private void AddMALM()
		{
			MALMItems.Add(new MALMItem { Title = $"New Task {++_count}", IsCompleted = false, CreateDate = DateTime.Now });
		}


		private void ToggleCompletion(MALMItem item)
		{
			item.IsCompleted = !item.IsCompleted;
			item.CompleteDate = DateTime.Now;
		}


		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


	}
}