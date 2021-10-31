import { ICampaign } from "./store/campaigns/campaigns.model";

export interface IAppState {
    campaigns: Map<string, ICampaign>;
}