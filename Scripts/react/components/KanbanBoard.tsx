
import React = require('react');

import { TicketStatus } from '../enumerations/TicketStatus';
import { ITicket } from '../contracts/ITicket';
import { KanbanBoardComponent, IKanbanBoardProps } from '../contracts/IKanbanBoard';
import { TicketNotFoundException } from '../exceptions/TicketExceptions';
import { Column } from './column';

export class KanbanBoard extends KanbanBoardComponent
{
    constructor(props: IKanbanBoardProps)
    {
        super(props);

        this.state =
            {
                tickets: new Array<ITicket>(),
                repo: this.props.ticketRepository,
                boardHasMounted: false
            }
    }

    componentDidMount()
    {
        this.state.repo.getAll()
            .then((response) => this.setState({ tickets: response, boardHasMounted: true }))
    }

    OnDrop(event: React.DragEvent, dropState: TicketStatus): void
    {
        let id = event.dataTransfer.getData("id");
        let targetTicket: ITicket;
        let tickets = this.state.tickets;
        let droppingTicketIndex =
            this.state.tickets.findIndex(
                ticket => Number(ticket.id) === Number(id))

        if (droppingTicketIndex === -1)
            throw new TicketNotFoundException();

        targetTicket = this.state.tickets[droppingTicketIndex];
        targetTicket.state = dropState;
        tickets[droppingTicketIndex] = targetTicket;

        this.state.repo.update(targetTicket);
        this.setState({ tickets: tickets });
    }

    render(): JSX.Element
    {   
        if (this.state.boardHasMounted) {
            return (
                <div className="flex-container">
                    <Column
                        wipLimit={5}
                        onDrop={this.OnDrop.bind(this)}
                        header={"Open"}
                        onDropStatus={TicketStatus.open}
                        tickets={this.state.tickets.filter((ticket) => ticket.state === TicketStatus.open)} />

                    <Column
                        wipLimit={5}
                        onDrop={this.OnDrop.bind(this)}
                        header={"Analysis"}
                        onDropStatus={TicketStatus.analysis}
                        tickets={this.state.tickets.filter((ticket) => ticket.state === TicketStatus.analysis)} />


                    <Column
                        wipLimit={5}
                        onDrop={this.OnDrop.bind(this)}
                        header={"Debugging"}
                        onDropStatus={TicketStatus.debugging}
                        tickets={this.state.tickets.filter((ticket) => ticket.state === TicketStatus.debugging)} />


                    <Column
                        wipLimit={5}
                        onDrop={this.OnDrop.bind(this)}
                        header={"Testing"}
                        onDropStatus={TicketStatus.testing}
                        tickets={this.state.tickets.filter((ticket) => ticket.state === TicketStatus.testing)} />


                    <Column
                        wipLimit={5}
                        onDrop={this.OnDrop.bind(this)}
                        header={"Resolved"}
                        onDropStatus={TicketStatus.resolved}
                        tickets={this.state.tickets.filter((ticket) => ticket.state === TicketStatus.resolved)} />
                </div>
            )
        }
        else
        {
            return (
                <div className="flex-container"></div>
               )
        }
    }
}
