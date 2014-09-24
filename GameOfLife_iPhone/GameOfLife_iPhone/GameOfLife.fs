module GameOfLife

type Cell =
    | Alive
    | Dead

let setBoard = 
    let rnd = new System.Random()
    Array2D.init 20 20 (fun i j ->  
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

let lifecycle (board: Cell[,]) = 
    Array2D.init (Array2D.length1 board) (Array2D.length2 board) (fun i j ->
        let neighbours = amount_neighbours board (i, j)
        match neighbours with
        | 2 -> board.[i,j]
        | 3 -> Alive
        | _ -> Dead)