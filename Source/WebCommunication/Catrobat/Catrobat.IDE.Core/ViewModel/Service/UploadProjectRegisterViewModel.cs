﻿using System;
using System.Globalization;
using Catrobat.IDE.Core.Utilities;
using Catrobat.IDE.Core.Utilities.JSON;
using Catrobat.IDE.Core.Services;
using Catrobat.IDE.Core.Services.Common;
using Catrobat.IDE.Core.Resources.Localization;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace Catrobat.IDE.Core.ViewModel.Service
{
    public class UploadProjectRegisterViewModel : ViewModelBase
    {
        public delegate void NavigationCallbackEvent();

        #region private Members

        private CatrobatContextBase _context;
        private MessageboxResult _missingLoginDataCallbackResult;
        private MessageboxResult _wrongLoginDataCallbackResult;
        private MessageboxResult _registrationSuccessfulCallbackResult;
        private string _username;
        private string _password;
        private string _email;

        #endregion

        #region Properties

        public CatrobatContextBase Context
        {
            get { return _context; }
            set { _context = value; RaisePropertyChanged(() => Context); }
        }
        public NavigationCallbackEvent NavigationCallback { get; set; }

        public string Username
        {
            get { return _username; }
            set
            {
                if (_username != value)
                {
                    _username = value;

                    RaisePropertyChanged(() => Username);
                }
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;

                    RaisePropertyChanged(() => Password);
                }
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    RaisePropertyChanged(() => Email);
                }
            }
        }

        #endregion

        #region Commands

        public RelayCommand RegisterCommand { get; private set; }

        #endregion

        #region Actions

        private async void RegisterAction()
        {
            // TODO implement register
            ServiceLocator.NavigationService.RemoveBackEntry();

            if (string.IsNullOrEmpty(_username) || string.IsNullOrEmpty(_password) /*|| string.IsNullOrEmpty(_email)*/)
            {
                ServiceLocator.NotifictionService.ShowMessageBox(AppResources.Main_UploadProjectLoginErrorCaption,
                    AppResources.Main_UploadProjectMissingLoginData, MissingLoginDataCallback, MessageBoxOptions.Ok);
            }
            else
            {
                //CatrobatWebCommunicationService.RegisterOrCheckToken(_username, _password, _email,
                //                                         ServiceLocator.CulureService.GetCulture().TwoLetterISOLanguageName,
                //                                         RegionInfo.CurrentRegion.TwoLetterISORegionName,
                //                                         UtilTokenHelper.CalculateToken(_username, _password),
                //                                         RegisterOrCheckTokenCallback);
                JSONStatusResponse status_response =  await CatrobatWebCommunicationService.AsyncLoginOrRegister(_username, _password, _email,
                                                             ServiceLocator.CulureService.GetCulture().TwoLetterISOLanguageName,
                                                             RegionInfo.CurrentRegion.TwoLetterISORegionName);

                //Context.CurrentToken = UtilTokenHelper.CalculateToken(_username, _password);
                // TODO store values if everything was sucessfull --> error handling (evt. include email here)
                Context.CurrentToken = status_response.token;
                Context.CurrentUserName = _username;

                if (status_response.statusCode == StatusCodes.ServerResponseRegisterOk)
                {
                    ServiceLocator.NotifictionService.ShowMessageBox(AppResources.Main_UploadProjectRegistrationSucessful,
                        string.Format(AppResources.Main_UploadProjectWelcome, _username), RegistrationSuccessfulCallback, MessageBoxOptions.Ok);
                }
                else if (status_response.statusCode == StatusCodes.ServerResponseTokenOk)
                {
                    if (NavigationCallback != null)
                    {
                        NavigationCallback();
                    }
                    else
                    {
                        //TODO: Throw error because of navigation callback shouldn't be null
                        throw new Exception("This error shouldn't be thrown. The navigation callback must not be null.");
                    }
                }
                else //Unknown error
                {
                    var messageString = string.IsNullOrEmpty(status_response.answer) ? string.Format(AppResources.Main_UploadProjectUndefinedError, status_response.statusCode.ToString()) :
                                            string.Format(AppResources.Main_UploadProjectLoginError, status_response.answer);

                    ServiceLocator.NotifictionService.ShowMessageBox(AppResources.Main_UploadProjectLoginErrorCaption,
                        messageString, WrongLoginDataCallback, MessageBoxOptions.Ok);
                }
            }
        }

        protected override void GoBackAction()
        {
            ResetViewModel();
            base.GoBackAction();
        }

        #endregion

        #region MessageActions
        private void ContextChangedAction(GenericMessage<CatrobatContextBase> message)
        {
            Context = message.Content;
        }
        #endregion
        public UploadProjectRegisterViewModel()
        {
            // Commands
            RegisterCommand = new RelayCommand(RegisterAction);

            Messenger.Default.Register<GenericMessage<CatrobatContextBase>>(this,
                 ViewModelMessagingToken.ContextListener, ContextChangedAction);

            NavigationCallback = navigationCallback;
        }

        #region Callbacks

        private void navigationCallback()
        {
            ServiceLocator.NavigationService.NavigateTo<UploadProjectViewModel>();
        }

        private void MissingLoginDataCallback(MessageboxResult result)
        {
            _missingLoginDataCallbackResult = result;
        }

        private void WrongLoginDataCallback(MessageboxResult result)
        {
            _wrongLoginDataCallbackResult = result;
        }

        private void RegistrationSuccessfulCallback(MessageboxResult result)
        {
            _registrationSuccessfulCallbackResult = result;

            if (result == MessageboxResult.Ok)
            {
                if (NavigationCallback != null)
                {
                    NavigationCallback();
                }
                else
                {
                    //TODO: Throw error because of navigation callback shouldn't be null
                    throw new Exception("This error shouldn't be thrown. The navigation callback must not be null.");
                }
            }
        }

        //private void RegisterOrCheckTokenCallback(bool registered, string errorCode, string statusMessage, string token)
        //{
        //    //Context.CurrentToken = UtilTokenHelper.CalculateToken(_username, _password);
        //    // TODO store values if everything was sucessfull --> error handling (evt. include email here)
        //    Context.CurrentToken = token;
        //    Context.CurrentUserName = _username;

        //    if (registered)
        //    {
        //        ServiceLocator.NotifictionService.ShowMessageBox(AppResources.Main_UploadProjectRegistrationSucessful,
        //            string.Format(AppResources.Main_UploadProjectWelcome, _username), RegistrationSuccessfulCallback, MessageBoxOptions.Ok);
        //    }
        //    else if (errorCode == StatusCodes.ServerResponseTokenOk.ToString())
        //    {
        //        if (NavigationCallback != null)
        //        {
        //            NavigationCallback();
        //        }
        //        else
        //        {
        //            //TODO: Throw error because of navigation callback shouldn't be null
        //            throw new Exception("This error shouldn't be thrown. The navigation callback must not be null.");
        //        }
        //    }
        //    else //Unknown error
        //    {
        //        var messageString = string.IsNullOrEmpty(statusMessage) ? string.Format(AppResources.Main_UploadProjectUndefinedError, errorCode) :
        //                                string.Format(AppResources.Main_UploadProjectLoginError, statusMessage);

        //        ServiceLocator.NotifictionService.ShowMessageBox(AppResources.Main_UploadProjectLoginErrorCaption,
        //            messageString, WrongLoginDataCallback, MessageBoxOptions.Ok);
        //    }
        //}

        #endregion

        private void ResetViewModel()
        {
            Username = "";
            Password = "";
            Email = "";
        }
    }
}