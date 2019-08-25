
import { InvalidTicketStateException } from '../exceptions/TicketExceptions';
import { TicketStatus } from '../enumerations/TicketStatus';
import { ITicket } from '../contracts/ITicket'
import { ITicketRepository } from '../contracts/ITicketRepository';
import { ApiTicket } from './ApiTicket';
import { TicketStateMapper } from '../enumerations/TicketStateMapper';


export class ApiTicketRepository implements ITicketRepository
{
    constructor(private ApiUrl: string) { };

    async getAll(): Promise<Array<ITicket>>
    {
        return fetch(this.ApiUrl)
            .then((resp) => resp.json())
            .then((JsonData) => ApiTicket.parseJsonArray(JsonData))
    }

    getByStatus(state: TicketStatus): Promise<Array<ITicket>>
    {
        if (!(state in TicketStatus))
            throw new InvalidTicketStateException();

        let stateApiString = TicketStateMapper.TicketStateToApiString(state);

        return fetch(this.ApiUrl + "?filter=" + stateApiString)
            .then((resp) => resp.json())
            .then((data) => ApiTicket.parseJsonArray(data))
            .then((data) => data.filter((ticket) => ticket.state === state));
    }

    getById(Id: number): Promise<ITicket> {

        return fetch(this.ApiUrl + "/" + Id)
            .then((resp) => resp.json())
            .then((data) => ApiTicket.parseJsonArray(data))
            .then((ticket) => { return ticket[0] });
    }

    update(ticket: ITicket): Promise<void>
    {
        return fetch(this.ApiUrl + "/" + ticket.id + '?newState=' + ticket.state,
            {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
            .then(response => console.log('Success:', JSON.stringify(response)))
            .catch(error => console.error('Error:', error));
    }
}
