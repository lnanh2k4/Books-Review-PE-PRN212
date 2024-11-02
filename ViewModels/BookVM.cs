using Books.Models;
using Books.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Books.ViewModels
{
    public class BookVM : BaseVM
    {
        private Book _selecteditem;
        public Book SelectedItem
        {
            get
            {
                return _selecteditem;
            }
            set
            {
                _selecteditem = value;
                OnPropertyChanged(nameof(SelectedItem));

                if (SelectedItem != null)
                {
                    NewItem = new Book
                    {
                        Author = SelectedItem.Author,
                        BookId = SelectedItem.BookId,
                        BookStatus = SelectedItem.BookStatus,
                        Category = SelectedItem.Category,
                        CategoryId = SelectedItem.CategoryId,
                        PublicDate = SelectedItem.PublicDate,
                        Title = SelectedItem.Title
                    };
                    OnPropertyChanged(nameof(NewItem));
                }
            }
        }

        private Book _newitem;
        public Book NewItem
        {
            get
            {
                return _newitem;
            }
            set
            {
                _newitem = value;
                OnPropertyChanged(nameof(NewItem));
            }
        }

        public ObservableCollection<Book> Books { get; set; }
        public ObservableCollection<Category> Categories { get; set; }

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand UpdateCommand { get; }

        public BookVM()
        {
            Books = new ObservableCollection<Book>();
            Categories = new ObservableCollection<Category>();
            NewItem = new Book();
            AddCommand = new RelayCommand(ADD);
            UpdateCommand = new RelayCommand(UPDATE);
            DeleteCommand = new RelayCommand(DELETE);
            Load();
        }

        private void DELETE(object obj)
        {
            using (var context = new BookContext())
            {
                if (SelectedItem != null)
                {
                    context.Books.Remove(SelectedItem);
                    context.SaveChanges();
                    Books.Remove(SelectedItem);
                    SelectedItem = null;
                    NewItem = new();
                    MessageBox.Show("Book is deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void UPDATE(object obj)
        {
            using (var context = new BookContext()) {
                if (SelectedItem != null) { 
                var item = context.Books.Where(x => x.BookId == SelectedItem.BookId).FirstOrDefault();
                    if (item != null)
                    {
                        item.Author = NewItem.Author;
                        item.PublicDate = NewItem.PublicDate;
                        item.Title = NewItem.Title;  
                        item.CategoryId = NewItem.CategoryId;
                        context.SaveChanges();
                        Load();
                        NewItem = new();
                        MessageBox.Show("Book is uppdated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        private void ADD(object obj)
        {
            using (var context = new BookContext()) {
                if (NewItem != null) {
                    var category = context.Categories.Where(x => x.CatId == NewItem.CategoryId).FirstOrDefault();
                    NewItem.Category = category;
                    NewItem.BookId = 0;
                    NewItem.BookStatus = 1;
                    context.Books.Add(NewItem);
                    context.SaveChanges();
                    Books.Add(NewItem);
                    NewItem = new Book();
                    MessageBox.Show("Book is added successfully!","Success",MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void Load()
        {
            using (var context = new BookContext())
            {
                var items = context.Books.ToList();
                Books.Clear();
                foreach (var item in items)
                {
                    Books.Add(item);
                }

                var categories = context.Categories.ToList();
                Categories.Clear();
                foreach (var item in categories)
                {
                    Categories.Add(item);
                }
            }
        }
    }
}
