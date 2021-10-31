import { createReducer, on } from '@ngrx/store';
import * as CampaignActions from './campaigns.actions';
import { ICampaign } from './campaigns.model';

export const initialState: Array<ICampaign> = [];

export const campaignsReducer = createReducer(
    initialState,
    on(CampaignActions.getCampaigns, (state, { campaigns }) => campaigns),
    on(CampaignActions.setCampaigns, (state, { campaigns }) => state = campaigns)
);