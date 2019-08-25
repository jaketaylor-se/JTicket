import * as React from 'react';
import * as ReactDOM from 'react-dom';

class InvalidTicketStateException extends Error {}
class TicketNotFoundException extends Error { }
class InvalidTicketSeverityException extends Error { }
class DuplicateTicketEntriesException extends Error { }


enum TicketType
{
    bug = 0,
    story = 1
}

enum TicketStatus
{

    open = 0,
    analysis = 1,
    debugging = 2,
    testing = 3,
    resolved = 4
}


enum TicketSeverity {

    veryLow = 1,
    low = 2,
    medium = 3,
    high = 4,
    veryHigh = 5
}


class TicketStateMapper {

    public static TicketStateToApiString(status: TicketStatus): string {

        switch (status) {

            case TicketStatus.open:
                return "open";
            case TicketStatus.analysis:
                return "analysis";
            case TicketStatus.debugging:
                return "debugging";
            case TicketStatus.testing:
                return "testing";
            case TicketStatus.resolved:
                return "resolved";
            default:
                throw new InvalidTicketStateException();
        }
    }

    public static TicketStateToNumber(status: TicketStatus): number {

        switch (status) {

            case TicketStatus.open:
                return 0;
            case TicketStatus.analysis:
                return 1;
            case TicketStatus.debugging:
                return 2;
            case TicketStatus.testing:
                return 3;
            case TicketStatus.resolved:
                return 4;
            default:
                throw new InvalidTicketStateException();
        }
    }

    public static TicketStateStringToState(status: string): TicketStatus {

        switch (status.toLowerCase()) {

            case "open":
                return TicketStatus.open;
            case "analysis":
                return TicketStatus.analysis;
            case "debugging":
                return TicketStatus.debugging;
            case "testing":
                return TicketStatus.testing;
            case "Resolved":
                return TicketStatus.resolved;
            default:
                throw new InvalidTicketStateException();
        }
    }
}

interface ITicket {

    State: TicketStatus;
    Id: number;
    CreationDate: Date;
    LastModified: Date;
    Title: string;
    Type: TicketType;
    Severity: TicketSeverity;
}

class Ticket implements ITicket {

    State: TicketStatus;
    Id: number;
    CreationDate: Date;
    LastModified: Date;
    Title: string;
    Type: TicketType;
    Severity: TicketSeverity;

    static parseJson(Json: Array<any>): Array<ITicket>{

        let tickets = new Array<Ticket>();

        for (let i = 0; i < Json.length; i++)
        {
            let ticket = new Ticket();

            ticket.CreationDate = Json[i].creationDate;
            ticket.LastModified = Json[i].lastModified;
            ticket.Title = Json[i].title;
            ticket.State = Json[i].state;
            ticket.Id = Json[i].id;
            ticket.Type = Json[i].type;
            ticket.Severity = Json[i].severity;

            tickets.push(ticket);
        }

        return tickets;
    }

}


interface ITicketRepository {

    getAll(): Promise<Array<ITicket>>;
    getByStatus(state: TicketStatus): Promise<Array<ITicket>>;
    getById(id: number): Promise<ITicket>;
    set(theTicket: ITicket): Promise<void>;
}


class ApiTicketRepository implements ITicketRepository
{
    constructor(private ApiUrl: string) { };

    async getAll(): Promise<Array<ITicket>>  {

        return fetch(this.ApiUrl)
            .then((resp) => resp.json())
            .then((JsonData) => Ticket.parseJson(JsonData))
    }

    getByStatus(state: TicketStatus): Promise<Array<ITicket>> {

        if (!(state in TicketStatus))
            throw new InvalidTicketStateException();

        let stateApiString = TicketStateMapper.TicketStateToApiString(state);

        return fetch(this.ApiUrl + "?filter=" + stateApiString)
            .then((resp) => resp.json())
            .then((data) => Ticket.parseJson(data))
            .then((data) => data.filter((ticket) => ticket.State === state));
    }

    getById(Id: number): Promise<ITicket> {

        return fetch(this.ApiUrl + "/" + Id)
            .then((resp) => resp.json())
            .then((data) => Ticket.parseJson(data))
            .then((ticket) => { return ticket[0] });
    }

    set(ticket: ITicket): Promise<void> {


        return fetch(this.ApiUrl + "/" + ticket.Id + '?newState=' + ticket.State,
            {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
            .then(response => console.log('Success:', JSON.stringify(response)))
            .catch(error => console.error('Error:', error));
    }

}


interface IEventHandler {

    onDrop(event: React.DragEvent, newState: TicketStatus): void;
    onDragStart(event: React.DragEvent, targetTicketId: number): void;
    onDragOver(event: React.DragEvent): void;
}


class EventHandler implements IEventHandler {

    constructor(private repository: ITicketRepository) {}

    async onDrop(event: React.DragEvent, newState: TicketStatus): Promise<void> {

        let id = event.dataTransfer.getData("id");

        let tickets = await this.repository.getAll();

        let ticketIndex = tickets.findIndex(ticket => Number(ticket.Id) === Number(id));

        if (ticketIndex === -1)
            throw new TicketNotFoundException();

        tickets[ticketIndex].State = newState;


        this.repository.set(tickets[ticketIndex]);
    }

    onDragStart(event: React.DragEvent, targetTicketId: number): void {

        let ticketIdString = String(targetTicketId);
        event.dataTransfer.setData("id", ticketIdString);
    }

    onDragOver(event: React.DragEvent) {
        event.preventDefault();
    }
}


interface ICardBuilder {

    create(aTicket: ITicket): JSX.Element;
    setHandler(handler: IEventHandler): void;
}

class CardBuilder implements ICardBuilder {

    constructor(private handler: IEventHandler) { }

    createTicketStateString(aStatus: TicketStatus) {

        switch (aStatus) {

            case TicketStatus.open:
                return "open";
            case TicketStatus.analysis:
                return "analysis";
            case TicketStatus.debugging:
                return "debugging";
            case TicketStatus.testing:
                return "testing";
            case TicketStatus.resolved:
                return "resolved";
            default:
                throw new InvalidTicketStateException();
        }
    }



    createTicketSeverityString(type: TicketSeverity) {

        switch (type) {

            case TicketSeverity.veryLow:
                return "Very Low";
            case TicketSeverity.low:
                return "Low";
            case TicketSeverity.medium:
                return "Medium";
            case TicketSeverity.high:
                return "High";
            case TicketSeverity.veryHigh:
                return "Very High";
            default:
                throw new InvalidTicketSeverityException();
        }
    }

    create(aTicket: ITicket): JSX.Element {

        let ticketClassName: string;
        let filePath: string;
        if (Number(aTicket.Type) === Number(TicketType.bug)) {
            ticketClassName = "bug";
            filePath = "../../Content/bug.png";
        }

        else {
            ticketClassName = "story";
            filePath = "../../Content/book.png";
        }

            

        return <a className="ticket-link" href={"http://localhost:53229/Ticket/Edit/" + aTicket.Id}>
            <div key={aTicket.Id}
                className={"draggable " + ticketClassName}
                draggable
                onDragStart={(e) => this.handler.onDragStart(e, aTicket.Id)}>

                <img src={filePath} alt="bug" />

                <span className="card-title">{aTicket.Title}<br /></span>
                <span className="card-severity">Priority: {this.createTicketSeverityString(aTicket.Severity)}<br /></span>
                <span> Created On: {new Date(aTicket.CreationDate).toLocaleDateString()} </span>


            </div>
        </a>
    }

    setHandler(handler: IEventHandler): void {

        this.handler = handler;

    }
}

interface IColumn {

    Title: string;
    Tickets: Array<ITicket>;
    onDropStatusCode: number;
    TicketLimit: number;

    add(ticket: ITicket): void;
    isRunningHot(): boolean;
}


class Column implements IColumn {

    Title: string;
    Tickets: Array<ITicket>;
    onDropStatusCode: number;
    TicketLimit: number;

    constructor(Title: string, onDropStatusCode: number, Tickets?: Array<ITicket>, TicketLimit?:number) {

        if (!(onDropStatusCode in TicketStatus))
            throw new InvalidTicketStateException();

        this.onDropStatusCode = onDropStatusCode;

        if (typeof (Tickets) === "undefined")    // replace with default param
            this.Tickets = new Array<ITicket>();
        else
            this.Tickets = Tickets;

        this.Title = Title;

        if (typeof (TicketLimit) === "undefined")    // replace with default param
            this.TicketLimit = 0;
        else
            this.TicketLimit = TicketLimit;
    }

    add(ticket: ITicket): void {

        this.Tickets.push(ticket);
    }

    isRunningHot(): boolean
    {
        return this.Tickets.length > this.TicketLimit;
    }
}

interface IColumnBuilder {

    create(column: IColumn): JSX.Element;
    setHandler(handler: IEventHandler): void;
}


class ColumnBuilder implements IColumnBuilder {

    constructor(private handler: IEventHandler,
        private ticketBuilder: ICardBuilder)
    {
        this.ticketBuilder.setHandler(this.handler);
    }

    createTicketsJsx(column: IColumn): Array<JSX.Element> {

        let ticketsJsx = new Array<JSX.Element>();
        for (let i = 0; i < column.Tickets.length; i++) {
            ticketsJsx.push(this.ticketBuilder.create(column.Tickets[i]));
        }

        return ticketsJsx;
    }

    setHandler(handler: IEventHandler): void {

        this.handler = handler;
    }

    create(column: IColumn): JSX.Element {

        let ticketCountCssClassName = "";
        if (column.isRunningHot())
            ticketCountCssClassName = "column-header-ticket-count-overloaded";
        else
            ticketCountCssClassName = "column-header-ticket-count";


        return <div className="flex-column"
            onDragOver={(event) => this.handler.onDragOver(event)}
            onDrop={(e) => this.handler.onDrop(e, column.onDropStatusCode)}>

            <h2 className="flex-heading">
                <span className="column-header-title">{column.Title}</span>
                <span className={ticketCountCssClassName}>{column.Tickets.length}/{column.TicketLimit}</span>
            </h2>
            {this.createTicketsJsx(column)}
        </div>
    }
}


interface IColumnRepository
{
    getAll(): Promise<Map<TicketStatus, IColumn>> ;
    getByStatus(ticketStatus: TicketStatus): Promise<IColumn>;
}


class ApiColumnRespository implements IColumnRepository {


    constructor(private ticketRepository: ITicketRepository) { }


    async getAll(): Promise<Map<TicketStatus, IColumn>> {

        let map = new Map();

        let open_tickets = await this.getByStatus(TicketStatus.open);
        map.set(TicketStatus.open, open_tickets);

        let analysis_tickets = await this.getByStatus(TicketStatus.analysis)
        map.set(TicketStatus.analysis, analysis_tickets);

        let debugging_tickets = await this.getByStatus(TicketStatus.debugging);
        map.set(TicketStatus.debugging, debugging_tickets);

        let testing_tickets = await this.getByStatus(TicketStatus.testing);
        map.set(TicketStatus.testing, testing_tickets);

        let resolved_tickets = await this.getByStatus(TicketStatus.resolved);
        map.set(TicketStatus.resolved, resolved_tickets);

        return map;
    }

    createTicketStatusToColumnTitleMap(): Map<number, string> {

        let map = new Map();
        map.set(TicketStatus.open, "Open");
        map.set(TicketStatus.analysis, "Analysis");
        map.set(TicketStatus.debugging, "Debugging");
        map.set(TicketStatus.testing, "Testing");
        map.set(TicketStatus.resolved, "Resolved");

        return map
    }


    createTicketStatusToColumnDropCodeMap(): Map<number, number> {

        let map = new Map();
        map.set(TicketStatus.open, 0);
        map.set(TicketStatus.analysis, 1);
        map.set(TicketStatus.debugging, 2);
        map.set(TicketStatus.testing, 3);
        map.set(TicketStatus.resolved, 4);

        return map
    }

    async getByStatus(ticketStatus: TicketStatus): Promise<IColumn>
    {
        let tickets = await this.ticketRepository.getByStatus(ticketStatus);
        let titleMap = this.createTicketStatusToColumnTitleMap();
        let dropCodeMap = this.createTicketStatusToColumnDropCodeMap();
        let title = titleMap.get(ticketStatus);
        let dropCode = dropCodeMap.get(ticketStatus);
        return new Column(title, dropCode, tickets, 5);
    }
}



class KanbanBoardEventHandler implements IEventHandler {

    constructor(private kanbanBoard: KanbanBoardComponent,
        private ticketRepository: ITicketRepository) { }

    onDrop(event: React.DragEvent, newState: TicketStatus): void
    {
        let id = event.dataTransfer.getData("id");
        let columnKey: TicketStatus;
        let theTicketIndex: number = -1;
        let droppedTicket: ITicket;

        let keys = Array.from(this.kanbanBoard.state.columns.keys())

        for (let i = 0; i < keys.length; i++) {

            let ticketIndex = this.kanbanBoard.state.columns.get(keys[i]).Tickets
                .findIndex(ticket => Number(ticket.Id) === Number(id));

            if (ticketIndex === -1)
                continue;
            else {

                theTicketIndex = ticketIndex;
                columnKey = keys[i];
                droppedTicket = this.kanbanBoard.state.columns.get(columnKey).Tickets[theTicketIndex];
                break;
            }
        }

        if (theTicketIndex === -1)
            throw new TicketNotFoundException();


        // YUCK!
        this.kanbanBoard.state.columns.get(columnKey).Tickets[theTicketIndex].State = newState;
        this.ticketRepository.set(
            this.kanbanBoard.state.columns.get(columnKey).Tickets[theTicketIndex]);

        this.kanbanBoard.state.columns.get(columnKey).Tickets.splice(theTicketIndex, 1);
        this.kanbanBoard.state.columns.get(newState).Tickets.push(droppedTicket);
        this.kanbanBoard.setState({ ...this.kanbanBoard.state })
    }
    onDragStart(event: React.DragEvent, targetTicketId: number): void {

        let ticketIdString = String(targetTicketId);
        event.dataTransfer.setData("id", ticketIdString);
    }

    onDragOver(event: React.DragEvent) {
        event.preventDefault();
    }
}



interface IState {

    columns: Map<TicketStatus, IColumn>;
    initialLoadingComplete: boolean;
}


interface IProps {

    columnRepository: IColumnRepository;
    columnBuilder: IColumnBuilder;
    ticketRepository: ITicketRepository;
}


class KanbanBoardComponent extends React.Component<IProps, IState> {


    constructor(props: IProps)
    {
        super(props);
        this.state = {

            columns: new Map<TicketStatus, IColumn>(),
            initialLoadingComplete: false
        };
        this.props.columnBuilder.setHandler(this.createEventHandler());
    }

    createEventHandler(): IEventHandler {

        return new KanbanBoardEventHandler(this, this.props.ticketRepository);
    }

  
    componentDidMount(): void {

        let promise = this.props.columnRepository.getAll();
        promise
            .then((map) => this.setState({ columns: map, initialLoadingComplete: true}))
    }


    render(): JSX.Element {

        if (this.state.initialLoadingComplete) {
            return (
                <div className="flex-container">
                    {this.props.columnBuilder.create(this.state.columns.get(TicketStatus.open))}
                    {this.props.columnBuilder.create(this.state.columns.get(TicketStatus.analysis))}
                    {this.props.columnBuilder.create(this.state.columns.get(TicketStatus.debugging))}
                    {this.props.columnBuilder.create(this.state.columns.get(TicketStatus.testing))}
                    {this.props.columnBuilder.create(this.state.columns.get(TicketStatus.resolved))}
                </div>)
        }
        else {
            return (
                <div className="flex-container">

                </div>) 
        }


    }
}


let ticketRepository = new ApiTicketRepository("http://localhost:53229/api/tickets");
let columnRespository = new ApiColumnRespository(ticketRepository);
let handler = new EventHandler(ticketRepository);

let ticketBuilder = new CardBuilder(handler);
let columnBuilder = new ColumnBuilder(handler, ticketBuilder);

ReactDOM.render(<KanbanBoardComponent columnRepository={columnRespository}
    columnBuilder={columnBuilder}
    ticketRepository={ticketRepository} />,
    document.getElementById('flex-container'));

