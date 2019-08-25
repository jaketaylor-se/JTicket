
import { IColumnProps,  ColumnComponent } from "../contracts/IColumn";
import React = require("react");
import { ITicket } from "../contracts/ITicket";
import { Card } from "./card";


export class Column extends ColumnComponent
{
    constructor(props: IColumnProps)
    {
        super(props);

        this.state = {

            tickets: this.props.tickets,
            wipLimit: this.props.wipLimit,
            header: this.props.header,
            onDropStatus: this.props.onDropStatus
        }
    }

    componentWillReceiveProps(newProps: IColumnProps): void
    {
        this.setState({ tickets: newProps.tickets });

    }

    getClassName(): string
    {
        return "";
    }

    onDragOver(event: React.DragEvent): void
    {
        event.preventDefault();
    }

    exceedsWipLimit(): boolean
    {
        return this.state.wipLimit < this.state.tickets.length;
    }

    getTicketCountCssClassName(): string 
    {

        if (this.exceedsWipLimit())
            return "column-header-ticket-count-overloaded";
        else
            return "column-header-ticket-count";
    }

    convertTicketToCard(ticket: ITicket): JSX.Element
    {
        return (
            <Card ticket={ticket}/>
            )
    }

    convertTicketsToCards(): Array<JSX.Element>
    {
        let cards = new Array<JSX.Element>();
        for (let i = 0; i < this.state.tickets.length; i++)
        {
            cards.push(this.convertTicketToCard(this.state.tickets[i]));
        }

        return cards;
    }

    render(): JSX.Element
    {
        return <div className="flex-column"
            onDragOver={(event) => this.onDragOver(event)}
            onDrop={(event) => this.props.onDrop(event, this.state.onDropStatus)}>

            <h2 className="flex-heading">
                <span className="column-header-title">{this.state.header}</span>
                <span className={this.getTicketCountCssClassName()}>
                    {this.state.tickets.length}/{this.state.wipLimit}
                </span>
            </h2>
            {this.convertTicketsToCards()}
    </div>
    }
}
