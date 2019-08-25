
import { TicketStatus } from '../enumerations/TicketStatus';
import { TicketSeverity } from '../enumerations/TicketSeverity';
import { TicketType } from '../enumerations/TicketType';
import { ITicket } from '../contracts/ITicket';

export class ApiTicket implements ITicket
{
    state: TicketStatus;
    id: number;
    creationDate: Date;
    lastModified: Date;
    title: string;
    type: TicketType;
    severity: TicketSeverity;

    static parseJsonArray(Json: Array<any>): Array<ITicket>
    {
        let tickets = new Array<ApiTicket>();

        for (let i = 0; i < Json.length; i++) {
            let ticket = new ApiTicket();

            ticket.creationDate = Json[i].creationDate;
            ticket.lastModified = Json[i].lastModified;
            ticket.title = Json[i].title;
            ticket.state = Json[i].state;
            ticket.id = Json[i].id;
            ticket.type = Json[i].type;
            ticket.severity = Json[i].severity;

            tickets.push(ticket);
        }

        return tickets;
    }

}
