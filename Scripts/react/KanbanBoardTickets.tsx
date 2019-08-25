
import * as React from 'react';
import * as ReactDOM from 'react-dom';

import { KanbanBoard } from "./components/KanbanBoard";
import { ApiTicketRepository } from "./implementations/ApiTicketRespository";

let repo = new ApiTicketRepository("http://localhost:53229/api/tickets");

ReactDOM.render(<KanbanBoard ticketRepository={repo} />,
    document.getElementById('flex-container'));