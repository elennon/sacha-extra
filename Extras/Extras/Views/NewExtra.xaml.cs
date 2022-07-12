using Extras.Models;
using Plugin.Media;
using Plugin.Permissions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Extras.Services;
using Plugin.Permissions.Abstractions;
using System.Reflection;
using System.IO.Compression;

namespace Extras.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewExtra : ContentPage
    {

        private Project currentProject = new Project();
        private string AppFolder => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public List<string> Pics { get; set; }
        public NewExtra()
        {
            InitializeComponent();
            BindingContext = new Extra();
            Device.BeginInvokeOnMainThread(async () => await AskForPermissions());
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            exDate.Date = DateTime.Today;
            currentProject = await App.Database.GetCurrentProjectAsync();
            if (currentProject == null)
            {
                await DisplayAlert("", "There is no project selected as current project. Please add a project and set it as current project.", "OK");
                await Shell.Current.GoToAsync(nameof(ProjectsPage));
            }
            else
            {
                siteName.Text = currentProject.ProjectName;
            }
        }
        
        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            try
            {
                var ext = new Extra();// = (Extra)BindingContext;
                ext.MyId = Guid.NewGuid().ToString();
                if (menNo.Text == null)
                {
                    await DisplayAlert("Not Saved", "You need to add number of men", "OK");
                    return;
                }
                ext.Men = Convert.ToInt16(menNo.Text);
                if (description.Text == null)
                {
                    await DisplayAlert("Not Saved", "You need to add a description", "OK");
                    return;
                }
                ext.Description = description.Text;
                ext.Date =  exDate.Date;
                if (hours.Text == null)
                {
                    await DisplayAlert("Not Saved", "You need to add number of hours", "OK");
                    return;
                }
                ext.Hours = Convert.ToDouble(hours.Text);
                ext.Rate = Convert.ToDouble(rate.Text);
                ext.JobSite = siteName.Text;
                if (siteArea.Text == null)
                {
                    await DisplayAlert("Not Saved", "You need to add the site area", "OK");
                    return;
                }
                ext.SiteArea = siteArea.Text;
                ext.ProjectId = currentProject.MyId;
                ext.WasSent = false;

                var extrs = await App.Database.GetExtrasAsync(currentProject.MyId);
                var tooBig = await CheckPhotosSize(extrs.FindAll(x => x.WasSent == false));
                if (tooBig)
                {
                    await DisplayAlert("Not Saved", "You need to go to 'Veiw ready to send' and send off that list before adding any more because the images zip file will be too big to email.", "OK");
                }
                else
                {
                    var iid = App.Database.SaveExtraAsync(ext);
                    if (Pics != null)
                    {
                        var piks = getPics(Pics);
                        int counter = 0;
                        foreach (var pik in piks.Item1)
                        {
                            Pics pc = new Pics
                            {
                                //Pic = pik,
                                ExtraId = ext.MyId,
                                FileName = piks.Item2[counter]
                            };
                            await App.Database.SavePicAsync(pc);
                            counter++;
                        }
                    }
                    await DisplayAlert("Saved!", "", "OK");
                }                
            }
            catch (Exception)
            {
                await DisplayAlert("Alert", "Exception: " + e.ToString(), "OK");
            }
        }
        private async Task<bool> CheckPhotosSize(List<Extra> extrs)
        {           
            Environment.SetEnvironmentVariable("MONO_URI_DOTNETRELATIVEORABSOLUTE", "true");
            var zipFile = Path.Combine(AppFolder, @"temp.zip");
            if (File.Exists(zipFile)) { File.Delete(zipFile); }
            using (var archive = ZipFile.Open(zipFile, ZipArchiveMode.Create))
            {
                int count = 0;
                foreach (var ext in extrs)
                {
                    var phts = await App.Database.GetPicsAsync(ext.MyId);
                    foreach (var pc in phts)
                    {
                        var et = pc.FileName.Split('.');
                        var nme = "tmp" + count.ToString() + "." + et[et.Length - 1];
                        archive.CreateEntryFromFile(pc.FileName, nme);
                        count++;
                    }
                }
            }
            var fi = new FileInfo(zipFile);
            if (fi.Length > 24000000)
            {
                return true;
            }
            return false;
        }
        private (List<byte[]>, List<string>) getPics(List<string> pics)
        {
            List<string> fnames = new List<string>();
            List<byte[]> bts = new List<byte[]>();
            foreach (var item in pics)
            {
                fnames.Add(item.ToString());
                bts.Add(File.ReadAllBytes(item));
            }
            return (bts, fnames) ;
        }

        private async void SelectImagesButton_Clicked(object sender, EventArgs e)
        {
            //Check users permissions.
            var storagePermissions = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();//(Permission.Storage);
            var photoPermissions = await CrossPermissions.Current.CheckPermissionStatusAsync<PhotosPermission>();
            if (storagePermissions == PermissionStatus.Granted && photoPermissions == PermissionStatus.Granted)
            {
                //If we are on iOS, call GMMultiImagePicker.
                if (Device.RuntimePlatform == Device.iOS)
                {
                    //If the image is modified (drawings, etc) by the users, you will need to change the delivery mode to HighQualityFormat.
                    bool imageModifiedWithDrawings = false;
                    if (imageModifiedWithDrawings)
                    {
                        await GMMultiImagePicker.Current.PickMultiImage(true);
                    }
                    else
                    {
                        await GMMultiImagePicker.Current.PickMultiImage();
                    }

                    MessagingCenter.Unsubscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectediOS");
                    MessagingCenter.Subscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectediOS", (s, images) =>
                    {
                        //If we have selected images, put them into the carousel view.
                        if (images.Count > 0)
                        {
                            //Pics = images;
                            //var compressed = CompressAllImages(images);
                            //Pics = compressed;
                            //ImgCarouselView.ItemsSource = compressed;
                            Pics = images;
                            ImgCarouselView.ItemsSource = images;
                        }
                    });
                }
                //If we are on Android, call IMediaService.
                else if (Device.RuntimePlatform == Device.Android)
                {
                    DependencyService.Get<IMediaService>().OpenGallery();
                    MessagingCenter.Unsubscribe<App, List<string>>(this, "ImagesSelectedAndroid");
                    MessagingCenter.Subscribe<App, List<string>>(this, "ImagesSelectedAndroid", (s, images) =>
                    {
                        if (images.Count > 0)
                        {
                            var compressed = CompressAllImages(images);
                            Pics = compressed;
                            ImgCarouselView.ItemsSource = compressed;
                        }
                    });
                }
            }
            else
            {
                await DisplayAlert("Permission Denied!", "\nPlease go to your app settings and enable permissions.", "Ok");
            }
        }

        private long checkImageSize(List<string> images)
        {
            long allBytees = 0;
            foreach (var fPath in images)
            {
                FileInfo fi = new FileInfo(fPath);
                if (fi.Exists)
                {
                    allBytees += fi.Length;
                }
            }
            return allBytees;
        }
        private List<string> CompressAllImages(List<string> totalImages)
        {
            int displayCount = 1;
            int totalCount = totalImages.Count;
            List<string> compressedImages = new List<string>();
            foreach (string path in totalImages)
            {
                compressedImages.Add(DependencyService.Get<ICompressImages>().CompressImage(path));
                displayCount++;
            }
            for (int i = 0; i < totalImages.Count; i++)
            {
                var ext = totalImages[i].Split('.');
                System.IO.FileInfo fi = new System.IO.FileInfo(compressedImages[i]);
                // Check if file is there  
                if (fi.Exists)
                {
                    // Move file with a new name. Hence renamed.  
                    fi.MoveTo(compressedImages[i] + "." + ext[ext.Length - 1]);
                }
                //System.IO.File.Move(compressedImages[i], compressedImages[i] + ext[ext.Length -1]);
                compressedImages[i] = compressedImages[i] + "." +  ext[ext.Length - 1];
            }
            return compressedImages;
        }

        //protected async void ResizeImage()
        //{
        //    var assembly = typeof(NewExtra).GetTypeInfo().Assembly;
        //    byte[] imageData;

        //    Stream stream = assembly.GetManifestResourceStream(ResourcePrefix + "OriginalImage.JPG");
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        stream.CopyTo(ms);
        //        imageData = ms.ToArray();
        //    }

        //    byte[] resizedImage = await ImageResizer.ResizeImage(imageData, 400, 400);

        //    this._photo.Source = ImageSource.FromStream(() => new MemoryStream(resizedImage));
        //}

        /// <summary>
        ///     Make sure Permissions are given to the users storage.
        /// </summary>
        /// <returns></returns>
        private async Task<bool> AskForPermissions()
        {
            try
            {
                await CrossMedia.Current.Initialize();

                var storagePermissions = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();
                var photoPermissions = await CrossPermissions.Current.CheckPermissionStatusAsync<PhotosPermission>();
                if (storagePermissions != PermissionStatus.Granted || photoPermissions != PermissionStatus.Granted)
                {
                    var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Storage, Permission.Photos });
                    storagePermissions = results[Permission.Storage];
                    photoPermissions = results[Permission.Photos];
                }

                if (storagePermissions != PermissionStatus.Granted || photoPermissions != PermissionStatus.Granted)
                {
                    await DisplayAlert("Permissions Denied!", "Please go to your app settings and enable permissions.", "Ok");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("error. permissions not set. here is the stacktrace: \n" + ex.StackTrace);
                return false;
            }
        }

        /// <summary>
        ///     Unsubsribe from the MessagingCenter on disappearing.
        /// </summary>
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectedAndroid");
            MessagingCenter.Unsubscribe<App, List<string>>((App)Xamarin.Forms.Application.Current, "ImagesSelectediOS");
            GC.Collect();
        }
    }
}