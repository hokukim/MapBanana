import { ICampaign } from "./store/campaigns/campaigns.model";
import { IMap } from "./store/maps/maps.model";

export interface IAppState {
    campaigns: Map<string, ICampaign>;
    campaign: ICampaign;
    maps: Map<string, IMap>;
    map: IMap;
}