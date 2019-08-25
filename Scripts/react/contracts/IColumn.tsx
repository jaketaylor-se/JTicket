
import { CardComponent } from './ICard';
import React = require('react');
import { ITicket } from './ITicket';
import { TicketStatus } from '../enumerations/TicketStatus';

export interface IColumnProps
{
    tickets: Array<ITicket>;
    wipLimit: number;
    header: string;
    onDrop(event: React.DragEvent, onDropStatus: TicketStatus): void;
    onDropStatus: TicketStatus;
}


export interface IColumnState
{
    tickets: Array<ITicket>
    wipLimit: number;
    header: string;
    onDropStatus: TicketStatus;
}

export abstract class ColumnComponent extends React.Component<IColumnProps, IColumnState>
{
    abstract getClassName(): string;
    abstract onDragOver(event: React.DragEvent): void
}