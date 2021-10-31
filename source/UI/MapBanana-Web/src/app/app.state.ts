import { ICampaign } from "./store/campaigns/campaigns.model";

export interface IAppState {
    campaigns: Array<ICampaign>;
}