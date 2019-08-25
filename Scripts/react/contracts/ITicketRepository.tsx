
import { TicketStatus } from '../enumerations/TicketStatus';
import { ITicket } from '../contracts/ITicket';

export interface ITicketRepository
{
    getAll(): Promise<Array<ITicket>>;
    getByStatus(state: TicketStatus): Promise<Array<ITicket>>;
    getById(id: number): Promise<ITicket>;
    update(ticket: ITicket): Promise<void>;
}
