
import { ITicket } from '../contracts/ITicket';
import React = require('react');

export interface ICardProps
{
    ticket: ITicket;
}


export interface ICardState
{
    ticket: ITicket;
}

export abstract class CardComponent extends React.Component<ICardProps, ICardState>
{
    abstract getClassName(): string;
    abstract onDragStart(event: React.DragEvent): void;
}