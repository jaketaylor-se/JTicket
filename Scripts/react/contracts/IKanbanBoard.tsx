
import { ITicket } from './ITicket';
import React = require('react');
import { ITicketRepository } from './ITicketRepository';
import { ColumnComponent } from './IColumn';
import { CardComponent } from './ICard';

export interface IKanbanBoardProps
{
    ticketRepository: ITicketRepository;
}


export interface IKanbanBoardState
{
    tickets: Array<ITicket>;
    repo: ITicketRepository;
    boardHasMounted: boolean;
}

export abstract class KanbanBoardComponent extends React.Component<IKanbanBoardProps, IKanbanBoardState>
{
    
}