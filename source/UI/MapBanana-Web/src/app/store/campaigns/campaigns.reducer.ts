import { createReducer, on } from '@ngrx/store';
import * as CampaignActions from './campaigns.actions';
import { ICampaign } from './campaigns.model';

export const initialState: Map<string, ICampaign> = new Map<string, ICampaign>();

export const campaignsReducer = createReducer(
    initialState,
    on(CampaignActions.setCampaigns, (state, { campaigns }) => state = campaigns)
);