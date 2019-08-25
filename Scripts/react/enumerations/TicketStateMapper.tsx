
import { TicketStatus } from './TicketStatus';
import { InvalidTicketStateException } from '../exceptions/TicketExceptions';


export class TicketStateMapper
{

    public static TicketStateToApiString(status: TicketStatus): string
    {

        switch (status) {

            case TicketStatus.open:
                return "open";
            case TicketStatus.analysis:
                return "analysis";
            case TicketStatus.debugging:
                return "debugging";
            case TicketStatus.testing:
                return "testing";
            case TicketStatus.resolved:
                return "resolved";
            default:
                throw new InvalidTicketStateException();
        }
    }

    public static TicketStateToNumber(status: TicketStatus): number
    {

        switch (status) {

            case TicketStatus.open:
                return 0;
            case TicketStatus.analysis:
                return 1;
            case TicketStatus.debugging:
                return 2;
            case TicketStatus.testing:
                return 3;
            case TicketStatus.resolved:
                return 4;
            default:
                throw new InvalidTicketStateException();
        }
    }

    public static TicketStateStringToState(status: string): TicketStatus
    {

        switch (status.toLowerCase()) {

            case "open":
                return TicketStatus.open;
            case "analysis":
                return TicketStatus.analysis;
            case "debugging":
                return TicketStatus.debugging;
            case "testing":
                return TicketStatus.testing;
            case "Resolved":
                return TicketStatus.resolved;
            default:
                throw new InvalidTicketStateException();
        }
    }
}
