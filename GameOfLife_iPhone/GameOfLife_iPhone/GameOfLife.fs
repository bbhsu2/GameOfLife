module GameOfLife
open System
open MonoTouch.UIKit
open MonoTouch.Foundation

type Cell =
    | Alive
    | Dead

let rnd = new Random()

let setBoard = 
    Array2D.init 10 10 (fun i j ->  
        match rnd.Next(1,100) with
        |x when x < 50 -> Alive
        |_ -> Dead)

let flatten board = 
    board |> Seq.cast<Cell> |> Seq.toArray

let amount_neighbours (board: Cell[,]) (pos: int * int) =
    let range (n: int) (limit: int) = 
        let min = if n < 1 then 0 else n - 1
        let max = if n > (limit - 2) then (limit - 1) else n + 1
        [min .. max]

    let x_range = range (fst pos) (Array2D.length1 board)
    let y_range = range (snd pos) (Array2D.length2 board)

    List.sum [for x in x_range ->
        List.sum [for y in y_range -> if board.[x, y] = Alive && (x, y) <> pos then 1 else 0]]

let lifecycle (board: Cell[,]) = 
    Array2D.init (Array2D.length1 board) (Array2D.length2 board) (fun i j ->
        let neighbours = amount_neighbours board (i, j)
        match neighbours with
            | 2 -> board.[i, j]
            | 3 -> Alive
            | _ -> Dead)

let rec process_game (board: Cell[,]) (n: int) =
    match n with
        | x when x > 0 -> 
            printfn "Iteration"
            printfn "%A" board
            process_game (lifecycle board) (n - 1)
        | _ -> 0

