﻿using Catrobat.IDE.Core.Services;
using Catrobat.IDE.Core.ViewModels;
using Catrobat.IDE.Core.ViewModels.Settings;

namespace Catrobat.IDE.WindowsPhone.Views.Settings
{
    public partial class SettingsLanguageView
    {
        private readonly SettingsLanguageViewModel _viewModel =
            ServiceLocator.ViewModelLocator.SettingsLanguageViewModel;

        protected override ViewModelBase GetViewModel() { return _viewModel; }

        public SettingsLanguageView()
        {
            InitializeComponent();
        }
    }
}