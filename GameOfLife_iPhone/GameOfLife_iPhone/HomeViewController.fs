namespace GameOfLife_iPhone

open System
open System.Drawing
open System.Collections.Generic
open MonoTouch.UIKit
open MonoTouch.Foundation

open GameOfLife

type HomeViewController() as this=
    inherit UIViewController()

    let viewWidth = 32
    let viewHeight = 32

    let fitnessLabel = new UILabel()

    let continuationToken = ref true

    let workingBoard = ref setBoard

    let mutable horizontalIndex = 0
    let mutable verticalIndex = 0
    let mutable views = 
        Array.init 100 (fun _ -> let mutable view = new UIView(this.View.Bounds)
                                 view.Frame <- new RectangleF(new PointF(float32(viewWidth * verticalIndex), 
                                                                         float32(viewHeight * horizontalIndex + 64)),
                                                              new SizeF(float32(viewWidth), 
                                                                        float32(viewHeight)))
                                 horizontalIndex <- horizontalIndex + 1
                                 if horizontalIndex = 9 then
                                    horizontalIndex <- 0
                                    verticalIndex <- verticalIndex + 1
                                 view)

    let refreshColors (fromBoard : Cell[,]) () = 
        Array.iter2(fun (x:Cell) (y:UIView) -> match x with
                                               |Alive -> y.BackgroundColor <- UIColor.Black
                                               |Dead -> y.BackgroundColor <- UIColor.White) (flatten fromBoard) views

    let StartAlgorithm = 
        EventHandler(fun _ _ -> this.NavigationItem.RightBarButtonItem.Enabled <- false
                                this.InvokeOnMainThread( fun() -> async {
                                while !continuationToken do
                                    workingBoard := (lifecycle !workingBoard)
                                    UIView.Animate(float(1.0f),
                                                   float(0.0f),
                                                   UIViewAnimationOptions.TransitionNone,
                                                   new NSAction( refreshColors !workingBoard ),
                                                   fun() -> ())
                                    do! Async.Sleep(1100)
                                    fitnessLabel.Text <- "Fitness: " + (viewWidth).ToString()} |> Async.StartImmediate))
    
    do
        this.Title <- "Game of Life"
        this.NavigationItem.BackBarButtonItem <- new UIBarButtonItem("", UIBarButtonItemStyle.Plain, handler=null)
        this.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Iterate", UIBarButtonItemStyle.Plain, StartAlgorithm), true)
        this.View.Frame <- UIScreen.MainScreen.Bounds
        this.View.BackgroundColor <- UIColor.White

        refreshColors !workingBoard ()

        this.View.AddSubviews views








        //
//        Array.iter2(fun (x:Cell) (y:UIView) -> match x with
//                                               |Alive -> y.BackgroundColor <- UIColor.Black
//                                               |Dead -> y.BackgroundColor <- UIColor.White) (flatten board) views


        //    let updateUI = fun _ _ ->
//
//        let rec process_game (board: Cell[,]) (n: int) =
//            if n > 0 then
//                this.InvokeOnMainThread( fun () -> 
//                Array.iter2(fun (x:Cell) (y:UIView) -> 
//                                                       match x with
//                                                       |Alive -> y.BackgroundColor <- UIColor.Black
//                                                       |Dead -> y.BackgroundColor <- UIColor.White) (flatten board) views
//                
//                process_game (lifecycle board) (n - 1)
//                )
//            else ()
//
//        process_game board 10
//
//
//    let iterate = EventHandler(updateUI)