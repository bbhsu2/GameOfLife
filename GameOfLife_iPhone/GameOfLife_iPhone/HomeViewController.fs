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

    let mutable views = 
        Array.init 100 (fun x -> new UIView(this.View.Bounds))
    
    let Frames = 
        let rectangles = new List<RectangleF>()
        for i = 0 to 9 do
            for j = 0 to 9 do
                rectangles.Add(new RectangleF(new PointF( float32(viewWidth * j), float32(viewHeight * i + 64)), new SizeF( float32(viewWidth), float32(viewHeight)))) 
            done
        done
        rectangles
//    let mutable views = 
//        Array.init 100 (fun _ -> let mutable view = new UIView(this.View.Bounds)
//                                 view.Frame <- new RectangleF(new PointF(float32(viewWidth * horizontalIndex), 
//                                                                         float32(viewHeight * verticalIndex + 64)),
//                                                              new SizeF(float32(viewWidth), 
//                                                                        float32(viewHeight)))
//                                 horizontalIndex <- horizontalIndex + 1
//                                 if horizontalIndex = 9 then
//                                    horizontalIndex <- 0
//                                    verticalIndex <- verticalIndex + 1
//                                 view)

    let refreshColors (fromBoard : Cell[,]) () = 
        let flatBoard = flatten fromBoard
        views |> Array.iteri(fun i x -> match flatBoard.[i] with
                                        |Alive -> x.BackgroundColor <- UIColor.Black
                                        |Dead -> x.BackgroundColor <- UIColor.White)
//        Array.iter2(fun (x:Cell) (y:UIView) -> match x with
//                                               |Alive -> y.BackgroundColor <- UIColor.Black
//                                               |Dead -> y.BackgroundColor <- UIColor.White) (flatten fromBoard) views

    let StartAlgorithm = 
        EventHandler(fun _ _ -> this.NavigationItem.RightBarButtonItem.Enabled <- false
                                this.InvokeOnMainThread( fun() -> async {
                                while !continuationToken do
                                    printfn "%A" !workingBoard
                                    workingBoard := (lifecycle !workingBoard)
                                    printfn "%A" !workingBoard
                                    UIView.Animate(float(0.6f),
                                                   float(0.0f),
                                                   UIViewAnimationOptions.TransitionNone,
                                                   new NSAction( refreshColors !workingBoard ),
                                                   fun() -> ())
                                    do! Async.Sleep(700)
                                    fitnessLabel.Text <- "Fitness: " + (viewWidth).ToString()} |> Async.StartImmediate))
    
    do
        this.Title <- "Game of Life"
        this.NavigationItem.BackBarButtonItem <- new UIBarButtonItem("", UIBarButtonItemStyle.Plain, handler=null)
        this.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Iterate", UIBarButtonItemStyle.Plain, StartAlgorithm), true)
        this.View.Frame <- UIScreen.MainScreen.Bounds
        this.View.BackgroundColor <- UIColor.White

        let flatBoard = flatten !workingBoard

        views
        |> Array.iteri(fun i x -> match flatBoard.[i] with
                                  |Alive -> x.BackgroundColor <- UIColor.Black
                                  |Dead -> x.BackgroundColor <- UIColor.White
                                  x.Frame <- Frames.[i])

        printfn "%A" !workingBoard
//
//        refreshColors !workingBoard ()
//
        this.View.AddSubviews views