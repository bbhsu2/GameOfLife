namespace GameOfLife_iPhone

open System
open MonoTouch.UIKit
open MonoTouch.Foundation

[<Register("AppDelegate")>]
type AppDelegate() = 
    inherit UIApplicationDelegate()

    let window = new UIWindow(UIScreen.MainScreen.Bounds)
    let mutable navigation:UINavigationController = null

    override this.FinishedLaunching(app, options) = 
        let hVC = new NotesViewController()
        navigation <- new UINavigationController(hVC)
        window.RootViewController <- navigation
        window.MakeKeyAndVisible()
        true

module Main = 
    [<EntryPoint>]
    let main args = 
        UIApplication.Main(args, null, "AppDelegate")
        0

