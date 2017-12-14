﻿using LewtzGUI.Data_Access;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;

namespace LewtzGUI.ViewModel
{
    class MainWindowViewModel : ViewModelBase
    {
        TableRepository MainDBContext;

        ObservableCollection<ItemRollerViewModel> _ItemRollers;
        public ObservableCollection<ItemRollerViewModel> ItemRollers
        {
            get
            {
                if (_ItemRollers == null)
                {
                    _ItemRollers = new ObservableCollection<ItemRollerViewModel>();
                    _ItemRollers.CollectionChanged += this.OnItemRollersChanged;
                }
                return _ItemRollers;
            }
        }

        RelayCommand _newRollerCommand;
        public ICommand AddItemRoller
        {
            get
            {
                if (_newRollerCommand == null)
                {
                    _newRollerCommand = new RelayCommand(
                       param => AddNewItemRoller(),
                       param => true
                       );
                }
                return _newRollerCommand;
            }
        }

        void AddNewItemRoller()
        {
            if (MainDBContext == null)
            {
                MainDBContext = new TableRepository();
            }
            _ItemRollers.Add(new ItemRollerViewModel(MainDBContext));
        }

        void OnItemRollersChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (ItemRollerViewModel itemroller in e.NewItems)
                    itemroller.RequestClose +=() => this.ItemRollers.Remove(sender as ItemRollerViewModel);

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (ItemRollerViewModel itemroller in e.OldItems)
                    itemroller.RequestClose -= () => this.ItemRollers.Remove(sender as ItemRollerViewModel);
        }

        void OnItemRollerRequestClose(object sender, EventArgs e)
        {
            this.ItemRollers.Remove(sender as ItemRollerViewModel);
        }
    }
}
