renderers are based on FPS (frame per second). the base renderer class will start a timer in another thread on `Loaded` event and closese that thread in `Unloaded` event automatically.
Regarding FPS set in the `UiRuntimeVariables`, it will call the `public abstract void BaseFpsRender.RenderFrame(UiDataRepositorySnapshot shot);`
and control should do render on each call.