using System;
using System.Collections.Generic;
using System.Diagnostics;
using Caliburn.Micro;
using Action = System.Action;

namespace AutoDumper.ViewModels
{
    public class ShellViewModel : PropertyChangedBase
    {
        public IEnumerable<ItemViewModel> Items { get; set; }

        public ShellViewModel()
        {
         
            Items = new List<ItemViewModel>
                        {
                            new ItemViewModel("1"),
                            new ItemViewModel("2"),
                            new ItemViewModel("3"),
                            new ItemViewModel("4"),
                            new ItemViewModel("5"),
                            new ItemViewModel("6"),
                            new ItemViewModel("7"),
                            new ItemViewModel("8"),
                            new ItemViewModel("9"),
                            new ItemViewModel("10"),
                        };
        }
    }

    public class ItemViewModel : Screen
    {
        public string S { get; private set; }
        public ItemViewModel(string s)
        {
            S = s;
        }
        public void DoThings()
        {
            Debug.WriteLine(S);
        }
    }
}
