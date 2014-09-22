module GameOfLife
open System
open MonoTouch.UIKit
open MonoTouch.Foundation

type Cell =
    | Alive
    | Dead

let rnd = new System.Random()

let setBoard = 
    Array2D.init 10 10 (fun i j ->  
        match rnd.Next(1,100) with
        |x when x < 50 -> Alive
        |_ -> Dead)

let flatten board = 
    board |> Seq.cast<Cell> |> Seq.toArray

let amount_neighbours (board: Cell[,]) (pos: int * int) =
    let alive board pos = 
        let (x, y) = pos
        if x < 0 || x >= Array2D.length1 board ||
           y < 0 || y >= Array2D.length2 board then
            false
        else
            board.[x, y] = Alive

    let vicinity x = seq { x - 1 .. x + 1 }

    seq {
        for x in vicinity (fst pos) do
        for y in vicinity (snd pos) do
        if (x, y) <> pos && alive board (x, y) then
            yield true
    } |> Seq.length
//let amount_neighbours (board: Cell[,]) (pos: int * int) =
//    let range (n: int) (limit: int) = 
//        let min = if n < 1 then 0 else n - 1
//        let max = if n > (limit - 2) then (limit - 1) else n + 1
//        [min .. max]
//
//    let x_range = range (fst pos) (Array2D.length1 board)
//    let y_range = range (snd pos) (Array2D.length2 board)
//
//    List.sum [for x in x_range ->
//        List.sum [for y in y_range -> if board.[x, y] = Alive && (x, y) <> pos then 1 else 0]]

let lifecycle (board: Cell[,]) = 
    Array2D.init (Array2D.length1 board) (Array2D.length2 board) (fun i j ->
        let neighbours = amount_neighbours board (i, j)
        match neighbours with
        | 2 -> board.[i,j]
        | 3 -> Alive
        | _ -> Dead)
//        match (board.[i,j], neighbours) with
//            | (_, 2) -> Alive//board.[i, j]
//            | (Alive, 3) -> Alive
//            | (Dead, 3) -> Alive
//            | (_, _) -> Dead)