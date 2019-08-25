
import { ICardProps, ICardState, CardComponent } from "../contracts/ICard";
import React = require("react");
import { TicketType } from "../enumerations/TicketType";
import { TicketStatus } from "../enumerations/TicketStatus";
import { TicketSeverity } from "../enumerations/TicketSeverity";
import { InvalidTicketSeverityException } from "../exceptions/TicketExceptions";

export class Card extends CardComponent
{
    constructor(props: ICardProps)
    {
        super(props);
        this.state =
            {
            ticket: this.props.ticket
            }
    }

    getClassName(): string
    {
        if (Number(this.state.ticket.type) === Number(TicketType.bug))
        {
            return "bug";
        }
        else
        {
            return "story";
        }
    }

    onDragStart(event: React.DragEvent): void
    {
        let ticketIdString = String(this.state .ticket.id);
        event.dataTransfer.setData("id", ticketIdString);
    }

    componentWillReceiveProps(newProps: ICardProps): void
    {
        this.setState({ ticket: newProps.ticket });
    }

    mapTicketSeverityToDisplayString(severity: TicketSeverity): string
    {
        switch (severity)
        {
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

    getIconImagePath(): string
    {
        if (this.state.ticket.type == TicketType.bug)
            return "../../Content/bug.png";
        else
            return "../../Content/book.png";
    }

    render(): JSX.Element
    {
        return <a className="ticket-link" href={"http://localhost:53229/Ticket/Edit/" + this.state.ticket.id}>
            <div key={this.state.ticket.id}
                className={"draggable " + this.getClassName()}
                draggable
                onDragStart={(e) => this.onDragStart(e)}>

                <img src={this.getIconImagePath()} alt="bug" />

                <span className="card-title">{this.state.ticket.title}<br /></span>
                <span className="card-severity">
                    Priority: {this.mapTicketSeverityToDisplayString(this.state.ticket.severity)}<br />
                </span>
                <span> Created On: {new Date(this.state.ticket.creationDate).toLocaleDateString()} </span>
            </div>
        </a>
    }
}
