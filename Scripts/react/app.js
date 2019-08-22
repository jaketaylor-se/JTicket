import React from 'react';
import ReactDOM from 'react-dom';

class TicketKanbanBoard extends React.Component {

    state = {
        tickets: []
    }

    componentDidMount()
    {
        fetch('http://localhost:53229/api/tickets')
            .then((resp) => resp.json())
            .then((data) => this.setState({ tickets: data }))
    }


    handleDragOver(ev) {
        ev.preventDefault();
    }


    handleDragStart(ev, ticketId) {
        ev.dataTransfer.setData("id", ticketId);
    }

    handleDropEvent(ev, stateCode) {
        let id = ev.dataTransfer.getData("id");
        let ticketIndex = this.state.tickets.findIndex(ticket => Number(ticket.id) === Number(id));
        let theTicket = this.state.tickets[ticketIndex];

        fetch('http://localhost:53229/api/tickets/' + id + '?newState=' + stateCode,
        {
            method: 'PUT', 
            body: JSON.stringify(theTicket), 
            headers: {
                    'Content-Type': 'application/json'
            }
        }).then(res => res.json())
          .then(response => console.log('Success:', JSON.stringify(response)))
          .catch(error => console.error('Error:', error));

        this.state.tickets[ticketIndex].state = stateCode;
        this.setState({ ...this.state });
    }


    createTicketJsx(ticket, ticketStateString) {

        return <a href={"http://localhost:53229/Ticket/Edit/" + ticket.id}>
            <div key={ticket.id}
                className={"draggable flex-" + ticketStateString + "-ticket"}
                        draggable
                        onDragStart={(e) => this.handleDragStart(e, ticket.id)}>

                        {ticket.title}
                    </div>
               </a>
    }

    createTicketColumnJsx(arrayOfTickets, columnTitle, dropEventStatusCode) {

        return <div className="flex-column"
                    onDragOver={(e) => this.handleDragOver(e)}
                    onDrop={(e) => this.handleDropEvent(e, dropEventStatusCode)}>

                    <h2 className="flex-heading">{columnTitle}</h2>
                        {arrayOfTickets}
               </div>
    }


    render() {

        let tickets =
        {
            open: this.state.tickets.filter(ticket => ticket.state === 0),
            resolved: this.state.tickets.filter(ticket => ticket.state === 4),
            analysis: this.state.tickets.filter(ticket => ticket.state === 1),
            debugging: this.state.tickets.filter(ticket => ticket.state === 2),
            testing: this.state.tickets.filter(ticket => ticket.state === 3)
        }

        let open_components = tickets.open.map(
            ticket => this.createTicketJsx(ticket, "open"))
        let resolved_components = tickets.resolved.map(
            ticket => this.createTicketJsx(ticket, "resolved"))
        let analysis_components = tickets.analysis.map(
            ticket => this.createTicketJsx(ticket, "analysis"))
        let debugging_components = tickets.debugging.map(
            ticket => this.createTicketJsx(ticket, "debugging"))
        let testing_components = tickets.testing.map(
            ticket => this.createTicketJsx(ticket, "testing"))

        return (
            <div className="flex-container">

                {this.createTicketColumnJsx(open_components, "Open", 0)}
                {this.createTicketColumnJsx(analysis_components, "Analysis", 1)}
                {this.createTicketColumnJsx(debugging_components, "Debugging", 2)}
                {this.createTicketColumnJsx(testing_components, "Testing", 3)}
                {this.createTicketColumnJsx(resolved_components, "Resolved", 4)}

            </div>
        );
    }
}

ReactDOM.render(<TicketKanbanBoard />, document.getElementById('flex-container'));

