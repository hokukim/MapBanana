import { createReducer, on } from '@ngrx/store';
import * as CampaignActions from './campaign.actions';
import { ICampaign } from './campaign.model';

export const initialState: ICampaign = {
    id: '',
    name: '',
    activeMapId: ''
};

export const campaignReducer = createReducer(
    initialState,
    on(CampaignActions.setCampaign, (state, { campaign }) => campaign )
);