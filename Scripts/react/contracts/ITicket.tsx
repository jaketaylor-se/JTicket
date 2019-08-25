
import { TicketStatus } from '../enumerations/TicketStatus';
import { TicketSeverity } from '../enumerations/TicketSeverity';
import { TicketType } from '../enumerations/TicketType';

export interface ITicket
{
    state: TicketStatus;
    id: number;
    creationDate: Date;
    lastModified: Date;
    title: string;
    type: TicketType;
    severity: TicketSeverity;
}
