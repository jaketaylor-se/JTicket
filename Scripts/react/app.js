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


    render() {

        let tickets =
        {
            open: this.state.tickets.filter(ticket => ticket.state === 0),
            resolved: this.state.tickets.filter(ticket => ticket.state === 4),
            analysis: this.state.tickets.filter(ticket => ticket.state === 1),
            debugging: this.state.tickets.filter(ticket => ticket.state === 2),
            testing: this.state.tickets.filter(ticket => ticket.state === 3)
        }

        let open_components = tickets.open.map(ticket =>
            <div key={ticket.id}
                className="draggable flex-open-ticket"
                draggable
                onDragStart={(e) => this.handleDragStart(e, ticket.id)}>

                {ticket.title}
            </div>)

        let resolved_components = tickets.resolved.map(ticket =>
            <div key={ticket.id}
                className="draggable flex-resolved-ticket "
                draggable
                onDragStart={(e) => this.handleDragStart(e, ticket.id)}>

                {ticket.title}
            </div>)

        let analysis_components = tickets.analysis.map(ticket =>
            <div key={ticket.id}
                className="draggable flex-analysis-ticket"
                draggable
                onDragStart={(e) => this.handleDragStart(e, ticket.id)}>
                {ticket.title}
            </div>)

        let debugging_components = tickets.debugging.map(ticket =>
            <div key={ticket.id}
                className="draggable flex-debugging-ticket "
                draggable
                onDragStart={(e) => this.handleDragStart(e, ticket.id)}>
                {ticket.title}
            </div>)

        let testing_components = tickets.testing.map(ticket =>
            <div key={ticket.id}
                className="draggable flex-testing-ticket"
                draggable
                onDragStart={(e) => this.handleDragStart(e, ticket.id)}>
                {ticket.title}
            </div>)

        return (
            <div className="flex-container">

                <div className="flex-column"
                    onDragOver={(e) => this.handleDragOver(e)}
                    onDrop={(e) => this.handleDropEvent(e, 0)}>

                    <h2 className="flex-heading">Open</h2>

                    {open_components}

                </div>

                <div className="flex-column"
                    onDragOver={(e) => this.handleDragOver(e)}
                    onDrop={(e) => this.handleDropEvent(e, 1)}>

                    <h2 className="flex-heading">Analysis</h2>
                    {analysis_components}

                </div>

                <div className="flex-column"
                    onDragOver={(e) => this.handleDragOver(e)}
                    onDrop={(e) => this.handleDropEvent(e, 2)}>

                    <h2 className="flex-heading">Debugging</h2>
                    {debugging_components}

                </div>

                <div className="flex-column"
                    onDragOver={(e) => this.handleDragOver(e)}
                    onDrop={(e) => this.handleDropEvent(e, 3)}>

                    <h2 className="flex-heading">Testing</h2>
                    {testing_components}
                </div>


                <div className="flex-column"
                    onDragOver={(e) => this.handleDragOver(e)}
                    onDrop={(e) => this.handleDropEvent(e, 4)}>

                    <h2 className="flex-heading">Resolved</h2>
                    {resolved_components}
                </div>
            </div>
        );
    }
}

ReactDOM.render(<TicketKanbanBoard />, document.getElementById('flex-container'));

