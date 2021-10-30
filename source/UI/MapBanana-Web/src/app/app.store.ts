import { ICampaign } from "./store/campaign/campaign.model";
import { IMap } from "./store/map/map.model";

export interface ICampaignsState {
    readonly campaigns: Map<string, ICampaign>;
}

export interface IAppState {
    readonly maps: Map<string, IMap>;
}