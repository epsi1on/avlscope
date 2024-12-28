# Adding new sidebar item

- Create a view (for example in the `Views` Folder)
- Create a view model (for example in the `ViewModels` Folder)
- Add the created pair of view and view model to `ViewLocation.cs` with `RegisterViewFactory()`
- Add the attribute to `App.axaml.cs`:

`internal static partial void ConfigureViewModels(IServiceCollection services);` and `internal static partial void ConfigureViews(IServiceCollection services);`


- Add the line to `MainViewModel.cs` in `_templates` field (currently line number 81)
`new ListItemTemplate(typeof(AboutViewModel), "AboutRegular", "About")`


done