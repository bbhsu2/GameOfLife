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

    let mutable population = 0

    let fitnessLabel = 
        new UILabel(new RectangleF(10.f, float32(64 + 10 * viewHeight + 10) , 140.f, 30.f))

    let populationLabel = 
        new UILabel(new RectangleF(10.f, fitnessLabel.Frame.Y + fitnessLabel.Frame.Height, 140.f, 30.f))

    let mutable horizontalIndex = 0
    let mutable verticalIndex = 0
    let mutable views = 
        Array.init 100 (fun x -> let mutable view = new UIView(this.View.Bounds)
                                 view.Frame <- new RectangleF(new PointF( float32(viewWidth * verticalIndex), float32(viewHeight * horizontalIndex + 64)), new SizeF( float32(viewWidth), float32(viewHeight)))
                                 view.BackgroundColor <- UIColor.Green

                                 horizontalIndex <- horizontalIndex + 1
                                 if horizontalIndex = 9 then
                                    horizontalIndex <- 0
                                    verticalIndex <- verticalIndex + 1

                                 view)

    let rnd = new Random()

    let mutable board =
        Array2D.init 10 10 (fun i j ->  match rnd.Next(1,100) with
                                        |x when x < 50 -> Alive
                                        |_ -> Dead)

    let newGeneration = 
        board <- (lifecycle board)
        board

    let flatten board = 
        board |> Seq.cast<Cell> |> Seq.toArray

    let updateUI = fun _ _ ->

        let rec process_game (board: Cell[,]) (n: int) =
            if n > 0 then
                this.InvokeOnMainThread( fun () -> 
                Array.iter2(fun (x:Cell) (y:UIView) -> 
                                                       match x with
                                                       |Alive -> y.BackgroundColor <- UIColor.Black
                                                       |Dead -> y.BackgroundColor <- UIColor.White) (flatten board) views
                
                process_game (lifecycle board) (n - 1)
                )
            else ()

        process_game board 10


    let iterate = EventHandler(updateUI)

    do
        this.Title <- "Game of Life"
        this.NavigationItem.BackBarButtonItem <- new UIBarButtonItem("", UIBarButtonItemStyle.Plain, handler=null)
        this.NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Iterate", UIBarButtonItemStyle.Plain, iterate), true)
        this.View.Frame <- UIScreen.MainScreen.Bounds
        this.View.BackgroundColor <- UIColor.White


        Array.iter2(fun (x:Cell) (y:UIView) -> match x with
                                               |Alive -> y.BackgroundColor <- UIColor.Black
                                               |Dead -> y.BackgroundColor <- UIColor.White) (flatten board) views

        this.View.AddSubviews views

