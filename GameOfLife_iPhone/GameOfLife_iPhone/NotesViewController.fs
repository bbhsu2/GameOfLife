namespace GameOfLife_iPhone

open System
open MonoTouch.UIKit
open MonoTouch.Foundation

type NotesTableSource(noteSelected:String->unit) = 
    inherit UITableViewSource()

    let cellID = "hi"

    member val Items = [||] with get, set

    override this.RowsInSection(tableview, section) = 
        if this.Items = [||] then 1 else this.Items.Length

    override this.RowSelected(tableView, indexPath) = 
        if not (this.Items = [||]) then
            noteSelected(this.Items.[indexPath.Row])
    
    override this.GetCell(tableView, indexPath) = 
        let cell = new UITableViewCell(UITableViewCellStyle.Subtitle, cellID)
        cell.TextLabel.Text <- this.Items.[indexPath.Row]
        cell


type NotesViewController() as this = 
    inherit UITableViewController()

    let source = new NotesTableSource(fun x -> this.NoteSelected(x))

    do
        source.Items <- [|"Hi"; "This"; "Is"; "A"; "Test"|]
        this.Title <- "My Notes"
        this.TableView.SeparatorStyle <- UITableViewCellSeparatorStyle.DoubleLineEtched
        this.TableView.RowHeight <- 55.0f
        this.TableView.Source <- source

    member val NoteSelected = (fun (x:String) -> ()) with get, set